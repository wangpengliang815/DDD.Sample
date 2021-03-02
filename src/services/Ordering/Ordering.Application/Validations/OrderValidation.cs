namespace Ordering.Application.Validations
{
    using System;

    using FluentValidation;

    using Ordering.Application.Commands;

    /// <summary>
    /// 定义基于 StudentCommand 的抽象基类 StudentValidation
    /// 继承 抽象类 AbstractValidator
    /// 注意需要引用 FluentValidation
    /// 注意这里的 T 是命令模型
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    public abstract class OrderValidation<T> : AbstractValidator<T>
        where T : OrderCommandArgs
    {
        //受保护方法，验证Name
        protected void ValidateName()
        {
            RuleFor(c => c.ConsigneeName)
                .NotEmpty().WithMessage("收货人姓名不能为空")//判断不能为空，如果为空则显示Message
                .Length(2, 10).WithMessage("姓名在2~10个字符之间");//定义 Name 的长度
        }

        //验证手机号
        protected void ValidatePhone()
        {
            RuleFor(c => c.ConsigneePhone)
                .NotEmpty()
                .Must(HavePhone)
                .WithMessage("手机号应该为11位！");
        }
#if debug
        // 验证Guid
        protected void ValidateId()
        {
            RuleFor(c => c.AggregateId)
                .NotEqual(Guid.Empty);
        }
#endif

        // 表达式
        protected static bool HaveMinimumAge(DateTime birthDate)
        {
            return birthDate <= DateTime.Now.AddYears(-14);
        }

        // 表达式
        protected static bool HavePhone(string phone)
        {
            return phone.Length == 11;
        }
    }
}
