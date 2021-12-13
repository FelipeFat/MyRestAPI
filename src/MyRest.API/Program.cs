using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyRest.Data.Context;
using MyRestAPI.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
    {
        c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    });
builder.Services.AddMvc();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{ 
    options.SuppressModelStateInvalidFilter = true; 
});
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.ResolveDependencies();
// Fix "A possible object cycle was detected which is not supported"
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
