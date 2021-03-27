using System;

using Microsoft.EntityFrameworkCore;

using Ordering.Infrastructure.Context;

namespace DotNetCore.Infra.UnitTests
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
