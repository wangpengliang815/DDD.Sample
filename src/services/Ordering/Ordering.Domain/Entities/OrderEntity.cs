using System.ComponentModel.DataAnnotations.Schema;

using DotnetCoreInfra.Abstractions;

namespace Ordering.Domain.Entities
{
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
    }
}
