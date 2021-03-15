using System.ComponentModel.DataAnnotations;

using MediatR;

using Newtonsoft.Json;

using Ordering.Application.Validations;
using Ordering.Domain.Enums;

namespace Ordering.Application.Commands
{
    public class OrderAddCommandArgs : OrderCommandArgs
        , IRequest<object>
    {
        [JsonIgnore]
        public override OrderStatus Status { get => base.Status; set => base.Status = value; }

        [JsonIgnore]
        public override string Id { get => base.Id; set => base.Id = value; }

        public override bool IsValid()
        {
            ValidationResult = new OrderAddCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
