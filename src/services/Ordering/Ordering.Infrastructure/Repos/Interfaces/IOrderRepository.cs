using System.Threading.Tasks;

using Ordering.Infrastructure.Entities;

namespace Ordering.Infrastructure.Repos
{
    public interface IOrderRepository
    {
        OrderEntity Create(OrderEntity entity);

        OrderEntity Update(OrderEntity entity);

        Task<OrderEntity> FindAsync(string id);

        OrderDetailEntity CreateDetail(OrderDetailEntity entity);
    }
}
