using FluentValidation.Results;
using MediatR;
using SalesMine.Core.Messages;
using System.Threading.Tasks;

namespace SalesMine.Core.Mediator
{
    public class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;

        public MediatorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublishEvent<T>(T evt) where T : Event
        {
            await _mediator.Publish(evt);
        }

        public async Task<ValidationResult> SendCommand<T>(T command) where T : Command
        {
            return await _mediator.Send(command);
        }
    }
}
