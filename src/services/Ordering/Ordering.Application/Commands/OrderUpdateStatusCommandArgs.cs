using MediatR;

using Ordering.Application.Validations;

namespace Ordering.Application.Commands
{
    public class OrderUpdateStatusCommandArgs : OrderCommandArgs
      , IRequest<object>
    {
        public override bool IsValid()
        {
            ValidationResult = new OrderUpdateStatusCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
