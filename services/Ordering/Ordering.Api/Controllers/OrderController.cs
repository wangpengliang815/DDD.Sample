using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Ordering.Domain.Entities;
using Ordering.Infrastructure.Context;

namespace Ordering.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ApplicationReadOnlyDbContext dbContext;

        public OrderController(ApplicationReadOnlyDbContext dbContext)
        {
            this.dbContext = dbContext;
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

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
