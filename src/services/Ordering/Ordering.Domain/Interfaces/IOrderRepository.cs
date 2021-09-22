namespace Ordering.Domain.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using Ordering.Domain.AggregateModels;

    public interface IOrderRepository
    {
        Order Add(Order order);

        void Update(Order order);

        Task<Order> Get(Guid id);
    }
}
