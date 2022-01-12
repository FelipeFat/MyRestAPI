using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyRest.Business.Intefaces;
using MyRestAPI.DTOs;
using MyRestAPI.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace MyRestAPI.Controllers
{
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;

        public AuthController(INotifier notifier, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IOptions<AppSettings> appSettings) : base(notifier)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register (RegisterUserViewModel registerUserViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser { UserName = registerUserViewModel.Email, Email = registerUserViewModel.Email, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(user, registerUserViewModel.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return CustomResponse(GenerateJwt());
            }
            foreach(var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(registerUserViewModel);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUserViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(loginUserViewModel.Email, loginUserViewModel.Password, false, true);

            if(result.Succeeded)
            {
                return CustomResponse(GenerateJwt());
            }
            if(result.IsLockedOut)
            {
                NotifyError("User blocked for invalid login attempts");
                return CustomResponse(loginUserViewModel);
            }

            NotifyError("User or password invalid");

            return CustomResponse(loginUserViewModel);
        }

        private async Task<string> GenerateJwt()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token); 
            return encodedToken;
        }
    }
}
