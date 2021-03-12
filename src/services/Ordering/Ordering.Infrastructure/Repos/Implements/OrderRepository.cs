namespace Ordering.Infrastructure.Repos
{
    using Ordering.Infrastructure.Context;
    using Ordering.Infrastructure.Entities;

    public class OrderRepository : BaseRepository<OrderEntity>,
        IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
