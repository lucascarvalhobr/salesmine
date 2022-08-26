using FluentValidation.Results;
using SalesMine.Core.Messages;
using System.Threading.Tasks;

namespace SalesMine.Core.Mediator
{
    public interface IMediatorHandler
    {
        Task PublishEvent<T>(T evt) where T : Event;

        Task<ValidationResult> SendCommand<T>(T command) where T : Command;
    }
}