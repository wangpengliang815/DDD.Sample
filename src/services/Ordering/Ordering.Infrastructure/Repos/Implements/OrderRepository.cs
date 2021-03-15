namespace Ordering.Infrastructure.Repos
{
    using System.Threading.Tasks;

    using Ordering.Infrastructure.Context;
    using Ordering.Infrastructure.Entities;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

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

        public async Task<OrderEntity> FindAsync(string id)
        {
            var entity = await DbContext.FindAsync<OrderEntity>(id).ConfigureAwait(false);
            if (entity is null)
            {
                return null;
            }
            DbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
    }
}
