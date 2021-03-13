using System;

using FluentValidation.Results;

using Newtonsoft.Json;

namespace DotnetCoreInfra.Abstractions
{
    /// <summary>
    /// 抽象命令基类
    /// </summary>
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
