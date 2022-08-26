using Microsoft.Extensions.Options;
using SalesMine.Gateways.Purchases.Extensions;
using System;
using System.Net.Http;

namespace SalesMine.Gateways.Purchases.Services
{
    public interface IOrderService
    {

    }

    public class OrderService : Service, IOrderService
    {
        private readonly HttpClient _httpClient;

        public OrderService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.OrderUrl);
        }
    }
}
