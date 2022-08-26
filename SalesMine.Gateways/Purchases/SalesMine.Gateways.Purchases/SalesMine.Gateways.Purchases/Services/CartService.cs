using Microsoft.Extensions.Options;
using SalesMine.Core.Communication;
using SalesMine.Gateways.Purchases.Extensions;
using SalesMine.Gateways.Purchases.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SalesMine.Gateways.Purchases.Services
{
    public interface ICartService
    {
        Task<CartDTO> GetCart();
        Task<ResponseResult> AddCartItem(CartItemDTO item);
        Task<ResponseResult> UpdateCartItem(Guid productId, CartItemDTO item);
        Task<ResponseResult> DeleteCartItem(Guid productId);
    }

    public class CartService : Service, ICartService
    {
        private readonly HttpClient _httpClient;

        public CartService(HttpClient httpClient, IOptions<AppServicesSettings> settings)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(settings.Value.CartUrl);
        }

        public async Task<CartDTO> GetCart()
        {
            var response = await _httpClient.GetAsync("/cart/");

            HandleResponseErrors(response);

            return await DeserializeResponseObject<CartDTO>(response);
        }

        public async Task<ResponseResult> AddCartItem(CartItemDTO item)
        {
            var itemContent = GetContent(item);

            var response = await _httpClient.PostAsync("/cart/", itemContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return Ok();
        }

        public async Task<ResponseResult> UpdateCartItem(Guid productId, CartItemDTO item)
        {
            var itemContent = GetContent(item);

            var response = await _httpClient.PutAsync($"/cart/{item.ProductId}", itemContent);

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return Ok();
        }

        public async Task<ResponseResult> DeleteCartItem(Guid productId)
        {
            var response = await _httpClient.DeleteAsync($"/cart/{productId}");

            if (!HandleResponseErrors(response)) return await DeserializeResponseObject<ResponseResult>(response);

            return Ok();
        }
    }
}
