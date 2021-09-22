namespace Ordering.Domain.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using DotNetCore.Infra.SeedWork;

    using MediatR;

    using Ordering.Domain.AggregateModels;
    using Ordering.Domain.Commands;
    using Ordering.Domain.Interfaces;

    public class OrderCommandHandler :
         IRequestHandler<OrderAddCommandArgs, object>
       , IRequestHandler<OrderUpdateStatusCommandArgs, object>
    {
        private readonly IOrderRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public OrderCommandHandler(IOrderRepository repository
            , IUnitOfWork unitOfWork)
        {
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>AddOrder Handle</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<object> Handle(OrderAddCommandArgs request
            , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            Order order = new Order(request.Id, request.TotalPrice, request.OrderItems);
            Order result = repository.Add(order);

            await unitOfWork.Commit();
            return await Task.FromResult(result);
        }

        /// <summary>OrderUpdateStatus Handle</summary>
        /// <param name="request">The request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from the request</returns>
        public async Task<object> Handle(OrderUpdateStatusCommandArgs request
           , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            Order order = await repository.Get(request.Id);
            if (order is null)
            {
                return await Task.FromResult($"request.Id{request.Id} 不存在");
            }
            else
            {
                repository.Update(order);
                return await Task.FromResult(order);
            }
        }
    }
}
