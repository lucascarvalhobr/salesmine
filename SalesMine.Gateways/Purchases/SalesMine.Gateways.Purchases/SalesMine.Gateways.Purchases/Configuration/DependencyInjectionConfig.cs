using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SalesMine.Core.Users;
using SalesMine.Gateways.Purchases.Extensions;
using SalesMine.Gateways.Purchases.Services;

namespace SalesMine.Gateways.Purchases.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthorizationDelegationHandler>();

            //services.AddHttpClient<ICatalogService, CatalogService>()
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegationHandler>()
            //    .AddTransientHttpErroPolicy()
        }
    }
}
