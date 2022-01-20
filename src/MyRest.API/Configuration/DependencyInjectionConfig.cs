using DevIO.Api.Extensions;
using Microsoft.Extensions.Options;
using MyRest.Business.Intefaces;
using MyRest.Business.Notificacoes;
using MyRest.Business.Services;
using MyRest.Data.Context;
using MyRest.Data.Repository;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MyRestAPI.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyDbContext>();
            services.AddScoped<INotifier, Notifier>();         

            services.AddScoped<ISupplierRepository, SupplierRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IProductService, ProductService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            return services;
        }
    }
}
