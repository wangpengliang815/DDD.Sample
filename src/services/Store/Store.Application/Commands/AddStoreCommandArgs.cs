namespace Store.Application.Commands
{
    using System.ComponentModel.DataAnnotations;

    using MediatR;

    using Store.Application.Validations;

    public class AddStoreCommandArgs : StoreCommandArgs
        , IRequest<object>
    {
        public override bool IsValid()
        {
            ValidationResult = new AddStorerCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
