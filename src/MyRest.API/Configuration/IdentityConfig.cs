using Microsoft.EntityFrameworkCore;
using MyRestAPI.Data;
using Microsoft.AspNetCore.Identity;

namespace MyRestAPI.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            _ = services.AddDbContext<ApplicationDbContext>(options =>
              {
                  options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
              });

            _ = services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return services;
        }
    }
}
