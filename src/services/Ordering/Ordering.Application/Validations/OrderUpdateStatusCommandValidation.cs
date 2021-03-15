namespace Ordering.Application.Validations
{
    using FluentValidation;

    using Ordering.Application.Commands;

    public class OrderUpdateStatusCommandValidation : AbstractValidator<OrderUpdateStatusCommandArgs>
    {
        public OrderUpdateStatusCommandValidation()
        {
            RuleFor(c => c.Id.Trim())
                .NotEmpty().WithMessage("Id is required");

            RuleFor(c => c.Status)
                .NotEmpty().WithMessage("Status is required");
        }
    }
}
