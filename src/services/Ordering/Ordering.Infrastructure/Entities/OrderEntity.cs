namespace Ordering.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using DotnetCoreInfra.Abstractions;

    [Table("T_Ordering_Order")]
    public class OrderEntity : BaseEntity
    {
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string ConsigneeName { get; private set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ConsigneePhone { get; private set; }

        /// <summary>Gets or sets the total price.</summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; set; }
    }
}
