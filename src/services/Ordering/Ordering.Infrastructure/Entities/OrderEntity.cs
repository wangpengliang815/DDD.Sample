namespace Ordering.Infrastructure.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    using DotNetCore.Infra.Abstractions;

    using Ordering.Domain.Enums;

    [Table("T_Ordering_Order")]
    public class OrderEntity : BaseEntity
    {
        /// <summary>
        /// 收货人名称
        /// </summary>
        public string ConsigneeName { get; set; }

        /// <summary>
        /// 收货人手机号
        /// </summary>
        public string ConsigneePhone { get; set; }

        /// <summary>Gets or sets the status.</summary>
        /// <value>The status.</value>
        public OrderStatus Status { get; set; }

        /// <summary>Gets or sets the total price.</summary>
        /// <value>The total price.</value>
        public decimal TotalPrice { get; set; }
    }
}
