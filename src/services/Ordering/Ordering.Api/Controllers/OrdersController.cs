using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Commands;
using Ordering.Infrastructure.Context;
using Ordering.Infrastructure.Entities;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private IMediator Mediator { get; }

        private readonly ApplicationDbContext dbContext;


        public OrdersController(ApplicationDbContext dbContext
            , IMediator mediator)
        {
            this.dbContext = dbContext;
            Mediator = mediator;
        }

        /// <summary>Odata</summary>
        /// <returns></returns>
        [HttpGet("odata")]
        [EnableQuery(PageSize = 100)]
        public ActionResult<IQueryable<OrderEntity>> Get()
        {
            return Ok(dbContext.Orders.AsQueryable());
        }

        /// <summary>创建订单</summary>
        /// <param name="command">The command.</param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] OrderAddCommandArgs command)
        {
            object result = await Mediator.Send(command);
            return new JsonResult(result);
        }

        /// <summary>更新状态</summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] OrderUpdateStatusCommandArgs command)
        {
            object result = await Mediator.Send(command);
            return new JsonResult(result);
        }
    }
}
