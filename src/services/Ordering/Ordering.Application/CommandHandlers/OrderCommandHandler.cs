namespace Ordering.Application.CommandHandlers
{
    using System.Threading;
    using System.Threading.Tasks;

    using AutoMapper;

    using MediatR;

    using Ordering.Application.Commands;
    using Ordering.Domain.AggregateModels;
    using Ordering.DomainService.Interfaces;

    public class OrderCommandHandler :
        IRequestHandler<OrderAddCommandArgs, object>
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderCommandHandler(IOrderService orderService
            , IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public async Task<object> Handle(OrderAddCommandArgs request
            , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            var result = await orderService.CreateAsync(mapper.Map<Order>(request));
            return await Task.FromResult(result);
        }

        public async Task<object> Handle(OrderUpdateStatusCommandArgs request
           , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return await Task.FromResult(request.ValidationResult);
            }
            var result = await orderService.UpdateAsync(mapper.Map<Order>(request));
            return await Task.FromResult(result);
        }
    }
}
