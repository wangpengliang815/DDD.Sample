namespace Ordering.Application.Validations
{
    using FluentValidation;

    using Ordering.Application.Commands;


    public class OrderUpdateStatusCommandValidation : AbstractValidator<OrderUpdateStatusCommandArgs>
    {
        public OrderUpdateStatusCommandValidation()
        {
           
        }
    }
}
