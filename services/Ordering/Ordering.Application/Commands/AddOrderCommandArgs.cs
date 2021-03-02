using System.ComponentModel.DataAnnotations;

using MediatR;

using Ordering.Application.Validations;

namespace Ordering.Application.Commands
{
    public class AddOrderCommandArgs : OrderCommandArgs
        , IRequest<bool>
    {
        public override bool IsValid()
        {
            ValidationResult = new AddOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
