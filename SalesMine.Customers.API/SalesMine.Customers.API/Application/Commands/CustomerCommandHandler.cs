using FluentValidation.Results;
using MediatR;
using SalesMine.Core.Messages;
using SalesMine.Customers.API.Application.Events;
using SalesMine.Customers.API.Models;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMine.Customers.API.Application.Commands
{
    public class CustomerCommandHandler : CommandHandler, IRequestHandler<AddCustomerCommand, ValidationResult>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCommandHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ValidationResult> Handle(AddCustomerCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var customer = new Customer(message.Id, message.Name, message.Email, message.Cpf);

            var foundCustomer = await _customerRepository.GetByCpf(customer.Cpf.Number);

            if (foundCustomer != null)
            {
                AddError("This CPF is already registered.");
                return ValidationResult;
            }

            _customerRepository.Create(customer);

            customer.AddEvent(new RegisteredCustomerEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistData(_customerRepository.UnitOfWork);
        }
    }
}
