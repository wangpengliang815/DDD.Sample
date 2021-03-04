using System;
using System.Linq;

using DotnetCoreInfra.Common;
using DotnetCoreInfra.Options;
using DotnetCoreInfra.UnitTests;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using MSTest.Extensions.Contracts;

using Ordering.Domain.Entities;

namespace DotnetCoreInfra.DataAccess.Tests
{
    [TestClass()]
    public class DataAccessorTests : TestBase
    {
        private DataAccessor<DbContext> dataAccessor;

        [TestMethod]
        public void Default()
        {
            Assert.IsTrue(true);
        }

        [TestInitialize]
        public void Init()
        {
            dataAccessor = new DataAccessor<DbContext>(TestMemoryDbContext
                , dataAccessorOptions);
        }

        [ContractTestCase]
        [TestCategory("passed")]
        public void FindAsyncTestMethod()
        {
            "When(id not exist),Result(success)".Test(() =>
            {
                dataAccessor.BatchInsertAsync(MockHelper.MockDataList<OrderEntity>())
                    .GetAwaiter()
                    .GetResult();
            });

            "When(id exist),Result(null)".Test(() =>
            {

            });
        }
    }
}