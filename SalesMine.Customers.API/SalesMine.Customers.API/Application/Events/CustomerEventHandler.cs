using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SalesMine.Customers.API.Application.Events
{
    public class CustomerEventHandler : INotificationHandler<RegisteredCustomerEvent>
    {
        public Task Handle(RegisteredCustomerEvent notification, CancellationToken cancellationToken)
        {
            //Send confirmation event
            return Task.CompletedTask;
        }
    }
}
