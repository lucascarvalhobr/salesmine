using FluentValidation;
using System;

namespace SalesMine.Carts.API.Models
{
    public class CartItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public decimal Value { get; set; }

        public string Image { get; set; }

        public Guid CartId { get; set; }

        public Cart Cart { get; set; }

        public CartItem()
        {

        }

        internal void SetCartId(Guid cartId)
        {
            CartId = cartId;
        }

        internal decimal CalculateValue()
        {
            return Quantity * Value;
        }

        internal void AddUnities(int unities)
        {
            Quantity += unities;
        }

        internal void UpdateUnities(int unities)
        {
            Quantity = unities;
        }

        internal bool IsValid()
        {
            return new CartItemValidation().Validate(this).IsValid;
        }

        public class CartItemValidation : AbstractValidator<CartItem>
        {
            public CartItemValidation()
            {
                RuleFor(c => c.ProductId)
                    .NotEqual(Guid.Empty)
                    .WithMessage("Product ID is invalid");

                RuleFor(c => c.Name)
                    .NotEmpty()
                    .WithMessage("Product name was not informed");

                RuleFor(c => c.Quantity)
                    .GreaterThan(0)
                    .WithMessage(item => $"Minimum quantity is 1 for item {item.Name}");

                RuleFor(c => c.Quantity)
                    .LessThan(15)
                    .WithMessage(item => $"Maximum quantity for item {item.Name} is {Cart.MAX_ITEM_QUANTITY}");

                RuleFor(c => c.Value)
                    .GreaterThan(0)
                    .WithMessage(item => $"Value for item {item.Name} must be greater than 0");
            }
        }
    }
}
