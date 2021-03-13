#if debug
namespace Ordering.Application.Validations
{

    using System;
    using System.Collections.Generic;
    using System.Text;

    using FluentValidation;

    using Ordering.Application.Commands;


    public class UpdateOrderCommandValidation : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidation()
        {
            ValidateId();
            ValidateName();
            ValidatePhone();
        }
    }
}
#endif
