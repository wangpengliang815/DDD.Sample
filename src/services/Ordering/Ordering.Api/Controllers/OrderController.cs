using System.Collections.Generic;
using System.Linq;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Ordering.Application.Commands;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Context;

namespace Ordering.Api.Controllers
{
    [Route("api/v1/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator mediator { get; }
        private readonly ApplicationReadOnlyDbContext dbContext;

        public OrderController(ApplicationReadOnlyDbContext dbContext
            , IMediator mediator)
        {
            this.dbContext = dbContext;
            this.mediator = mediator;
        }

        /// <summary>Get Orders</summary>
        /// <returns></returns>
        [HttpGet("odata/orders")]
        public ActionResult<IQueryable<OrderEntity>> Get()
        {
            return dbContext.Orders;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        /// <summary>Posts Order</summary>
        /// <param name="command">The command.</param>
        [HttpPost]
        public void Post([FromBody] AddOrderCommandArgs command)
        {
            mediator.Send(command);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
