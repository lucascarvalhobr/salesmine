using Microsoft.Extensions.DependencyInjection;
using SalesMine.Catalog.API.Data;
using SalesMine.Catalog.API.Data.Repository;
using SalesMine.Catalog.API.Models;

namespace SalesMine.Catalog.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
            return services;
        }
    }
}
