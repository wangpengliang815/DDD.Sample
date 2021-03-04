using System;

using Microsoft.EntityFrameworkCore;

using Ordering.Infrastructure.Context;

namespace DotnetCoreInfra.UnitTests
{
    public class TestBase
    {
        protected TestBase() { }

        /// <summary>
        /// 内存数据库Context
        /// 切记:因为Context在Dal中不会手动Dispose()
        /// 所以在每个测试用例中一定要手动清除当前用例构建的测试数据
        /// 否则可能出现单个测试运行正常,运行全部测试时,某些断言无法通过的问题
        /// </summary>
        protected static ApplicationDbContext TestMemoryDbContext =>
            new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options);
    }
}
