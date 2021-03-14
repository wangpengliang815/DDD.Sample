using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;

using Store.Infrastructure.Context;

namespace Store.Api.Controllers
{
    [Route("api/v1/Stores")]
    [ApiController]
    public class StoresController : ControllerBase
    {
        private IMediator Mediator { get; }

        private readonly ApplicationDbContext dbContext;

        public StoresController(ApplicationDbContext dbContext
            , IMediator mediator)
        {
            this.dbContext = dbContext;
            Mediator = mediator;
        }
    }
}
