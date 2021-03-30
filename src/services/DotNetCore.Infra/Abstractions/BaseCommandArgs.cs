using System;

using FluentValidation.Results;

using Newtonsoft.Json;

namespace DotNetCore.Infra.Abstractions
{
    public abstract class BaseCommandArgs
    {
        [JsonIgnore]
        public virtual DateTime Timestamp { get; set; }

        [JsonIgnore]
        //验证结果，需要引用FluentValidation
        public virtual ValidationResult ValidationResult { get; set; }

        protected BaseCommandArgs()
        {
            Timestamp = DateTime.Now;
        }

        //定义抽象方法，是否有效
        public abstract bool IsValid();
    }
}
