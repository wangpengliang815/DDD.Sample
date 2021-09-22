namespace Ordering.Infrastructure.Repos
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using Ordering.Domain.AggregateModels;
    using Ordering.Domain.Interfaces;
    using Ordering.Infrastructure.Context;

    public class OrderRepository : IOrderRepository
    {
        protected readonly ApplicationDbContext DbContext;

        public OrderRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public Order Add(Order order)
        {
            return DbContext.Orders.Add(order).Entity;
        }

        public void Update(Order order)
        {
            DbContext.Orders.Update(order);
        }

        public async Task<Order> Get(Guid id)
        {
            var entity = await DbContext.FindAsync<Order>(id).ConfigureAwait(false);
            if (entity is null)
            {
                return null;
            }
            DbContext.Entry(entity).State = EntityState.Detached;
            return entity;
        }
    }
}
