using System.Linq;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Ordering.Application.Commands;
using Ordering.Infrastructure.Entities;
using Ordering.Infrastructure.Context;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator Mediator { get; }

        private readonly ApplicationDbContext dbContext;

        public OrderController(ApplicationDbContext dbContext
            , IMediator mediator)
        {
            this.dbContext = dbContext;
            Mediator = mediator;
        }

        /// <summary>Get Orders</summary>
        /// <returns></returns>
        [HttpGet("odata")]
        public ActionResult<IQueryable<OrderEntity>> Get()
        {
            return dbContext.Orders;
        }

        /// <summary>Posts Order</summary>
        /// <param name="command">The command.</param>
        [HttpPost]
        public void Post([FromBody] AddOrderCommandArgs command)
        {
            Mediator.Send(command);
        }
    }
}
