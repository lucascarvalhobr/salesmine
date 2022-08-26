using Microsoft.Extensions.Options;
using SalesMine.Gateways.Purchases.Extensions;
using SalesMine.Gateways.Purchases.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesMine.Gateways.Purchases.Services
{
    public interface ICatalogService
    {
        Task<ProductItemDTO> GetById(Guid id);
    }

    public class CatalogService : Service, ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CatalogUrl);
        }

        public async Task<ProductItemDTO> GetById(Guid id)
        {
            var response = await _httpClient.GetAsync($"/catalog/products/{id}");

            HandleResponseErrors(response);

            return await DeserializeResponseObject<ProductItemDTO>(response);
        }
    }
}
