using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesMine.Core.Extensions;
using SalesMine.Customers.API.Services;

namespace SalesMine.Customers.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
            services.AddHostedService<CustomerRegisterListener>();
        }
    }
}
