using Ordering.Domain.AggregateModels;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Context;
using Ordering.Infrastructure.Repositories;

namespace DDDSample.Services.Ordering.Infrastructure.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity>,
        IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
