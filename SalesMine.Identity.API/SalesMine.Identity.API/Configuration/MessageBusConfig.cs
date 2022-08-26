using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SalesMine.Core.Extensions;

namespace SalesMine.Identity.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"));
        }
    }
}
