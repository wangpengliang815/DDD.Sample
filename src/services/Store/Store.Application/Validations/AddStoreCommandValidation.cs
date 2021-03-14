namespace Store.Application.Validations
{
    using FluentValidation;

    using Store.Application.Commands;

    public class AddStorerCommandValidation : AbstractValidator<StoreCommandArgs>
    {
        public AddStorerCommandValidation()
        {
            ValidateConsignee();
        }
        protected void ValidateConsignee()
        {
            // Method intentionally left empty.
        }
    }
}
