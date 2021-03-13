namespace Ordering.Domain.Enums
{
    using System.ComponentModel;

    public enum OrderStatus
    {
        /// <summary>
        /// 失效
        /// </summary>
        [Description("Inactived")]
        Inactived = 0,

        /// <summary>
        /// 活动
        /// </summary>
        [Description("Active")]
        Active = 1,
    }
}
