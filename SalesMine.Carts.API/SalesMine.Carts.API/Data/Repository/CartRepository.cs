using Microsoft.EntityFrameworkCore;
using SalesMine.Carts.API.Models;
using SalesMine.Core.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SalesMine.Carts.API.Data.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly CartContext _context;

        public IUnitOfWork UnitOfWork => _context;

        public CartRepository(CartContext context)
        {
            _context = context;
        }

        public void Create(Cart cart)
        {
            _context.Carts.Add(cart);
        }

        public void Update(Cart cart)
        {
            _context.Carts.Update(cart);
        }

        public async Task<Cart> GetCart(Guid customerId)
        {
            return await _context.Carts
                .Include(c => c.Itens)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        }

        public void CreateItemInCart(Cart cart, CartItem cartItem)
        {
            cart.Itens.Add(cartItem);

            _context.Carts.Update(cart);
        }

        public async Task<CartItem> GetCartItem(Guid cartId, Guid productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
        }

        public void UpdateItemInCart(Cart cart, CartItem cartItem)
        {
            var item = cart.Itens.First(i => i.Id == cartItem.Id);
            item = cartItem;

            _context.Carts.Update(cart);
        }

        public void RemoveCartItem(Cart cart, CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            _context.Carts.Update(cart);
        }
    }
}
