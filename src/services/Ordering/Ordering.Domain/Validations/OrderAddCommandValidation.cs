﻿namespace Ordering.Domain.Validations
{
    using FluentValidation;

    using Ordering.Domain.Commands;

    public class OrderAddCommandValidation : AbstractValidator<OrderAddCommandArgs>
    {
        public OrderAddCommandValidation()
        {
            //RuleFor(c => c.Consignee.Name)
            //    .NotEmpty().WithMessage("收货人姓名不能为空")
            //    .Length(2, 10).WithMessage("姓名在2~10个字符之间");

            //RuleFor(c => c.Consignee.Phone)
            //   .NotEmpty().WithMessage("手机号不能为空")
            //   .Must((string phone) =>
            //   {
            //       return phone.Length == 11;
            //   })
            //  .WithMessage("手机号应该为11位！");
        }
    }
}
