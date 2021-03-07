#define inMemoryDatabase
using System;
using System.Collections.Generic;

using DotnetCoreInfra.Common;
using DotnetCoreInfra.DataAccess;
using DotnetCoreInfra.DataAccessInterface;
using DotnetCoreInfra.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

using Ordering.Domain.Entities;
using Ordering.Infrastructure.Context;

namespace DotnetCoreInfra.UnitTests
{
    [TestClass()]
    public class DataAccessorTests : TestBase
    {
        private readonly ServiceCollection serviceCollection = new ServiceCollection();

        private IDataAccessor dataAccessor;

        private readonly string testConnectString =
            "Server=.;Database=DDDSample.DataBase.Test;uid=sa;pwd=wpl19950815;";

        [TestMethod]
        public void Default()
        {
            Assert.IsTrue(true);
        }

        [TestInitialize]
        public void Init()
        {
            Console.WriteLine($"Configure the connection string if using a real database: {testConnectString}");
            ServiceProvider provider = serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options
#if inMemoryDatabase
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
#else
                .UseSqlServer(testConnectString);
#endif
            })
            .AddOptions().Configure<DataAccessorOptions>(options =>
            {
                options.SaveImmediately = true;
            })
            .AddScoped<IDataAccessor, DataAccessor<ApplicationDbContext>>()
            .BuildServiceProvider();
            dataAccessor = provider.GetService<IDataAccessor>();
        }

        [ContractTestCase]
        [TestCategory("passed")]
        public void FindAsyncTestMethod()
        {
            "When(id exist),Result(success)".Test(() =>
            {
                var entity = MockHelper.MockEntity<OrderEntity>();
                dataAccessor.InsertAsync(entity).GetAwaiter().GetResult();
                var result = dataAccessor.FindAsync<OrderEntity>(entity.Id).GetAwaiter().GetResult();
                Assert.AreEqual(entity.Id, result.Id);
            });

            "When(id not exist),Result(null)".Test(() =>
            {
                var result = dataAccessor.FindAsync<OrderEntity>(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
                Assert.IsNull(result);
            });
        }
    }
}