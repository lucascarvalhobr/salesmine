using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SalesMine.Core.Bus;
using SalesMine.Core.Mediator;
using SalesMine.Core.Messages.Integration;
using SalesMine.Customers.API.Application.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMine.Customers.API.Services
{
    public class CustomerRegisterListener : BackgroundService
    {
        private readonly IMessageBus _messageBus;
        private readonly IServiceProvider _serviceProvider;

        public CustomerRegisterListener(IServiceProvider serviceProvider, IMessageBus messageBus)
        {
            _serviceProvider = serviceProvider;
            _messageBus = messageBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();

            return Task.CompletedTask;
        }

        private async Task<ResponseMessage> RegisterCustomer(RegisteredUserIntegrationEvent message)
        {
            var addCustomerCommand = new AddCustomerCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult result;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                result = await mediator.SendCommand(addCustomerCommand);
            }

            return new ResponseMessage(result);
        }

        private void SetResponder()
        {
            _messageBus.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>
                 (
                     request => RegisterCustomer(request).Result
                 );

            _messageBus.AdvancedBus.Connected += OnConnect;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }
    }
}
