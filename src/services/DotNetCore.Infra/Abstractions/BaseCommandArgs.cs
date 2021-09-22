namespace DotNetCore.Infra.Abstractions
{
    using System;

    using FluentValidation.Results;

    public abstract class BaseCommandArgs
    {
        [System.Text.Json.Serialization.JsonIgnore]
        public virtual DateTime Timestamp { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        //验证结果，需要引用FluentValidation
        public virtual ValidationResult ValidationResult { get; set; }

        protected BaseCommandArgs()
        {
            Timestamp = DateTime.Now;
        }

        // 定义抽象方法，是否有效
        public abstract bool IsValid();
    }
}
