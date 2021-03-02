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
        IRequestHandler<AddOrderCommandArgs, bool>
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderCommandHandler(IOrderService orderService
            , IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public Task<bool> Handle(AddOrderCommandArgs request
            , CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                return Task.FromResult(false);
            }
            orderService.CreateAsync(mapper.Map<Order>(request));
            return Task.FromResult(true);
        }
    }
}
