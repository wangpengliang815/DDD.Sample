using System;

using FluentValidation.Results;

namespace DotnetCoreInfra.Abstractions
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
    public abstract class BaseCommandArgs
    {
        //时间戳
        public DateTime Timestamp { get; private set; }

        //验证结果，需要引用FluentValidation
        public ValidationResult ValidationResult { get; set; }

        protected BaseCommandArgs()
        {
            Timestamp = DateTime.Now;
        }

        //定义抽象方法，是否有效
        public abstract bool IsValid();
    }
}
