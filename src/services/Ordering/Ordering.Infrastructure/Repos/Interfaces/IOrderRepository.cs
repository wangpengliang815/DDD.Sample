using Ordering.Infrastructure.Entities;

namespace Ordering.Infrastructure.Repos
{
    public interface IOrderRepository
    {
        OrderEntity Create(OrderEntity entity);

        OrderEntity Update(OrderEntity entity);

        OrderDetailEntity CreateDetail(OrderDetailEntity entity);
    }
}
