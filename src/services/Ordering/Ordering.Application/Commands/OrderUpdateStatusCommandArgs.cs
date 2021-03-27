using System.ComponentModel.DataAnnotations;

using DotNetCore.Infra.Abstractions;

using MediatR;

using Ordering.Application.Validations;
using Ordering.Domain.Enums;

namespace Ordering.Application.Commands
{
    public class OrderUpdateStatusCommandArgs : BaseCommandArgs
      , IRequest<object>
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        public override bool IsValid()
        {
            ValidationResult = new OrderUpdateStatusCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
