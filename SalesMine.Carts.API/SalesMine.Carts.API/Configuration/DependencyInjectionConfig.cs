using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SalesMine.Core.Users;

namespace SalesMine.Carts.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();
        }
    }
}
