using System.Threading;
using System.Threading.Tasks;

using Ordering.Domain.AggregateModels;

namespace Ordering.DomainService.Interfaces
{
    public interface IOrderService
    {
        Task<Order> CreateAsync(
              Order model);

        Task<Order> UpdateAsync(
              Order model);
    }
}
