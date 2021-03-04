namespace Ordering.Application.Commands
{
    using System.ComponentModel.DataAnnotations;

    using DotnetCoreInfra.Abstractions;

    public abstract class OrderCommandArgs : BaseCommandArgs
    {
        [Required]
        public string ConsigneeName { get; protected set; }

        [Required]
        public string ConsigneePhone { get; protected set; }
    }
}
