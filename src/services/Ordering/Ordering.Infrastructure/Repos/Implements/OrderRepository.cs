namespace Ordering.Infrastructure.Repos
{
    using Ordering.Infrastructure.Context;
    using Ordering.Infrastructure.Entities;

    public class OrderRepository : IOrderRepository
    {
        protected readonly ApplicationDbContext DbContext;

        public OrderRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public OrderEntity Create(OrderEntity entity)
        {
            DbContext.Orders.Add(entity);
            return entity;
        }
        public OrderEntity Update(OrderEntity entity)
        {
            DbContext.Orders.Update(entity);
            return entity;
        }

        public OrderDetailEntity CreateDetail(OrderDetailEntity entity)
        {
            DbContext.OrderDetails.Add(entity);
            return entity;
        }
    }
}
