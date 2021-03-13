using System.ComponentModel.DataAnnotations;

using MediatR;

using Newtonsoft.Json;

using Ordering.Application.Validations;
using Ordering.Domain.Enums;

namespace Ordering.Application.Commands
{
    public class AddOrderCommandArgs : OrderCommandArgs
        , IRequest<object>
    {
        [JsonIgnore]
        public override OrderStatus Status { get => base.Status; set => base.Status = value; }

        [JsonIgnore]
        public override string Guid { get => base.Guid; set => base.Guid = value; }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}
