using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesMine.Carts.API.Models;
using SalesMine.Core.Controller;
using SalesMine.Core.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMine.Carts.API.Controllers
{
    [Authorize]
    public class CartController : BaseController
    {
        private readonly IAspNetUser _user;
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository, IAspNetUser user)
        {
            _user = user;
            _cartRepository = cartRepository;
        }

        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            Cart customerCart = await GetCustomerCart();
            return CustomResponse(customerCart);
        }

        [HttpPost("cart")]
        public async Task<IActionResult> AddItemToCart(CartItem cartItem)
        {
            Cart cart = await GetCustomerCart();

            if (cart is null)
                CreateNewCart(cartItem);
            else
                UpdateExistingCart(cart, cartItem);

            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();

            await PersistData();

            return CustomResponse();
        }

        [HttpPut("cart/{productId}")]
        public async Task<IActionResult> UpdateCartItem(Guid productId, CartItem item)
        {
            var cart = await GetCustomerCart();
            var cartItem = await GetValidatedCartItem(productId, cart, item);

            if (cartItem == null) return CustomResponse();

            cart.UpdateUnities(cartItem, item.Quantity);

            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();

            _cartRepository.Update(cart);

            await PersistData();

            return CustomResponse();
        }

        [HttpDelete("cart/{productId}")]
        public async Task<IActionResult> DeleteCartItem(Guid productId)
        {
            var cart = await GetCustomerCart();
            var cartItem = await GetValidatedCartItem(productId, cart);

            if (cartItem == null) return CustomResponse();

            ValidateCart(cart);
            if (!IsValidOperation()) return CustomResponse();

            _cartRepository.RemoveCartItem(cart, cartItem);

            await PersistData();

            return CustomResponse();
        }

        private async Task<Cart> GetCustomerCart()
        {
            return await _cartRepository.GetCart(_user.GetUserId()) ?? new Cart();
        }

        private void CreateNewCart(CartItem cartItem)
        {
            var cart = new Cart(_user.GetUserId());
            cart.AddItem(cartItem);

            _cartRepository.Create(cart);
        }

        private void UpdateExistingCart(Cart cart, CartItem item)
        {
            var isItemAlreadyInCart = cart.IsItemAlreadyInCart(item);

            cart.AddItem(item);

            if (isItemAlreadyInCart)
            {
                _cartRepository.UpdateItemInCart(cart, item);
            }
            else
            {
                _cartRepository.CreateItemInCart(cart, item);
            }
        }

        private async Task<CartItem> GetValidatedCartItem(Guid productId, Cart cart, CartItem item = null)
        {
            if (item != null && productId != item.ProductId)
            {
                AddProcessingError("Invalid item");
                return null;
            }

            if (cart == null)
            {
                AddProcessingError("Cart not found");
                return null;
            }

            var cartItem = await _cartRepository.GetCartItem(cart.Id, productId);

            if (cartItem == null || !cart.IsItemAlreadyInCart(cartItem))
            {
                AddProcessingError("Item is already in cart");
                return null;
            }

            return cartItem;
        }

        private bool ValidateCart(Cart cart)
        {
            if (cart.IsValid()) return true;

            cart.ValidationResult.Errors.ToList().ForEach(e => AddProcessingError(e.ErrorCode));

            return false;
        }

        private async Task PersistData()
        {
            var result = await _cartRepository.UnitOfWork.Commit();
            if (!result) AddProcessingError("It was not possible to persist data");
        }
    }
}
