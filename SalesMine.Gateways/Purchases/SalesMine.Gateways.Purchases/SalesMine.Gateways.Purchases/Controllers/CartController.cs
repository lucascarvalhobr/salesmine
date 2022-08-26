using Microsoft.AspNetCore.Mvc;
using SalesMine.Core.Controller;
using SalesMine.Gateways.Purchases.Models;
using SalesMine.Gateways.Purchases.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMine.Gateways.Purchases.Controllers
{

    public class CartController : BaseController
    {
        private readonly ICartService _cartService;
        private readonly ICatalogService _catalogService;

        public CartController(ICartService cartService, ICatalogService catalogService)
        {
            _cartService = cartService;
            _catalogService = catalogService;
        }

        [HttpGet]
        [Route("purchases/cart")]
        public async Task<IActionResult> Index()
        {
            return CustomResponse(await _cartService.GetCart());
        }

        [HttpGet]
        [Route("purchases/cart-quantity")]
        public async Task<int> GetCartQuantity()
        {
            var quantity = await _cartService.GetCart();

            return quantity.Items?.Sum(i => i.Quantity) ?? 0;
        }

        [HttpPost]
        [Route("purchases/cart/items")]
        public async Task<IActionResult> AddCartItem(CartItemDTO item)
        {
            var product = await _catalogService.GetById(item.ProductId);

            await ValidateCartItem(product, item.Quantity);
            if (!IsValidOperation()) return CustomResponse();

            item.Name = product.Name;
            item.Value = product.Value;
            item.Image = product.Image;

            var response = await _cartService.AddCartItem(item);

            return CustomResponse(response);
        }

        [HttpPut]
        [Route("purchases/cart/items/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItemDTO item)
        {
            var product = await _catalogService.GetById(item.ProductId);

            await ValidateCartItem(product, item.Quantity);
            if (!IsValidOperation()) return CustomResponse();

            var response = await _cartService.UpdateCartItem(productId, item);

            return CustomResponse(response);
        }

        [HttpDelete]
        [Route("purchases/cart/items/{productId}")]
        public async Task<IActionResult> DeleteCartItem(Guid productId)
        {
            var product = await _catalogService.GetById(productId);

            if (product == null)
            {
                AddProcessingError("Invalid product!");
                return CustomResponse();
            }

            var response = await _cartService.DeleteCartItem(productId);

            return CustomResponse(response);
        }

        private async Task ValidateCartItem(ProductItemDTO product, int quantity)
        {
            if (product == null) AddProcessingError("Invalid product");
            if (quantity < 1) AddProcessingError($"Choose at least one unity of the product {product.Name}");

            var cart = await _cartService.GetCart();
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == product.Id);

            if (cartItem != null && cartItem.Quantity + quantity > product.StockQuantity)
            {
                AddProcessingError($"There is no stock available for the product {product.Name}. There are only {product.StockQuantity} unities in stock");
                return;
            }

            if (quantity > product.StockQuantity) AddProcessingError($"There is no stock available for the product {product.Name}. There are only {product.StockQuantity} unities in stock");
        }
    }
}
