using FluentValidation.Results;

namespace SalesMine.Core.Messages.Integration
{
    public class ResponseMessage
    {
        public ValidationResult ValidationResult { get; set; }

        public ResponseMessage(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
        }
    }
}
