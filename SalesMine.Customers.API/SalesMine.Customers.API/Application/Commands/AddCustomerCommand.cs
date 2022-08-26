using FluentValidation;
using SalesMine.Core.DomainObjects;
using SalesMine.Core.Messages;
using System;

namespace SalesMine.Customers.API.Application.Commands
{
    public class AddCustomerCommand : Command
    {
        public Guid Id { get; private set; }

        public string Name { get; private set; }

        public string Email { get; private set; }

        public string Cpf { get; private set; }

        public AddCustomerCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddCustomerValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class AddCustomerValidation : AbstractValidator<AddCustomerCommand>
    {
        public AddCustomerValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Invalid customer ID");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Customer name was not informed");

            RuleFor(c => c.Cpf)
                .Must(HaveValidCpf)
                .WithMessage("Informed CPF is invalid");

            RuleFor(c => c.Email)
                .Must(HaveValidEmail)
                .WithMessage("Informed email is invalid");
        }

        protected static bool HaveValidCpf(string cpf)
        {
            return Cpf.Validate(cpf);
        }

        protected static bool HaveValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
