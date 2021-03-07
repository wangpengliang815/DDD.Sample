using System;

using Microsoft.EntityFrameworkCore;

using Ordering.Infrastructure.Context;

namespace DotnetCoreInfra.UnitTests
{
    public class TestBase
    {
        protected TestBase()
        {
            
        }

        /// <summary>
        /// �ڴ����ݿ�Context
        /// </summary>
        protected static ApplicationDbContext TestMemoryDbContext
            => new ApplicationDbContext(
                new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }
}
