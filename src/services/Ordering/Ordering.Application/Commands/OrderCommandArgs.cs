namespace Ordering.Application.Commands
{
    using System.ComponentModel.DataAnnotations;

    using DotnetCoreInfra.Commands;

    public abstract class OrderCommandArgs : CommandArgs
    {
        [Required]
        public string ConsigneeName { get; protected set; }

        [Required]
        public string ConsigneePhone { get; protected set; }
    }
}
