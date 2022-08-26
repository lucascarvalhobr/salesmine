using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesMine.Carts.API.Models
{
    public class Cart
    {
        public const int MAX_ITEM_QUANTITY = 15;

        public Guid Id { get; set; }

        public Guid CustomerId { get; set; }

        public decimal TotalValue { get; set; }

        public List<CartItem> Itens { get; set; } = new List<CartItem>();

        public ValidationResult ValidationResult { get; set; }

        public Cart(Guid customerId)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
        }

        public Cart() { }

        internal void CalculateCartValue()
        {
            TotalValue = Itens.Sum(item => item.CalculateValue());
        }

        internal bool IsItemAlreadyInCart(CartItem item)
        {
            return Itens.Any(i => i.ProductId == item.ProductId);
        }

        internal CartItem GetItemByProductId(Guid productId)
        {
            return Itens.FirstOrDefault(p => p.ProductId == productId);
        }

        internal void AddItem(CartItem item)
        {
            item.SetCartId(Id);

            if (IsItemAlreadyInCart(item))
            {
                var existingItem = GetItemByProductId(item.ProductId);
                existingItem.AddUnities(item.Quantity);

                item = existingItem;
                Itens.Remove(existingItem);
            }

            Itens.Add(item);

            CalculateCartValue();
        }

        internal void UpdateItem(CartItem item)
        {
            item.SetCartId(Id);

            var existingItem = GetItemByProductId(item.ProductId);

            Itens.Remove(existingItem);
            Itens.Add(item);

            CalculateCartValue();
        }

        internal void RemoveItem(CartItem item)
        {
            var existingItem = GetItemByProductId(item.ProductId);
            Itens.Remove(existingItem);

            CalculateCartValue();
        }

        internal void UpdateUnities(CartItem item, int unities)
        {
            item.UpdateUnities(unities);
            UpdateItem(item);
        }

        internal bool IsValid()
        {
            var errors = Itens.SelectMany(i => new CartItem.CartItemValidation().Validate(i).Errors).ToList();
            errors.AddRange(new CartValidation().Validate(this).Errors);

            ValidationResult = new ValidationResult(errors);

            return ValidationResult.IsValid;
        }

        public class CartValidation : AbstractValidator<Cart>
        {
            public CartValidation()
            {
                RuleFor(c => c.CustomerId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Invalid customer");

                RuleFor(c => c.Itens.Count)
                    .GreaterThan(0)
                    .WithMessage("Cart doesn't have itens");

                RuleFor(c => c.TotalValue)
                    .GreaterThan(0)
                    .WithMessage("Cart total value must be greater than 0");
            }
        }
    }
}
