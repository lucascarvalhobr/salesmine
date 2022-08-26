using SalesMine.Core.Data;
using System;
using System.Threading.Tasks;

namespace SalesMine.Carts.API.Models
{
    public interface ICartRepository
    {
        IUnitOfWork UnitOfWork { get; }

        Task<Cart> GetCart(Guid customerId);

        void Create(Cart cart);

        void Update(Cart cart);

        Task<CartItem> GetCartItem(Guid cartId, Guid productId);

        void CreateItemInCart(Cart cart, CartItem cartItem);

        void UpdateItemInCart(Cart cart, CartItem cartItem);

        void RemoveCartItem(Cart cart, CartItem cartItem);
    }
}
