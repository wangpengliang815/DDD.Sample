namespace Ordering.Domain.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using MediatR;

    using Ordering.Application.Validations;

    public class OrderUpdateStatusCommandArgs : OrderCommandArgs
      , IRequest<object>
    {
        [Required]
        public override Guid Id { get => base.Id; set => base.Id = value; }

        public override bool IsValid()
        {
            ValidationResult = new OrderUpdateStatusCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
