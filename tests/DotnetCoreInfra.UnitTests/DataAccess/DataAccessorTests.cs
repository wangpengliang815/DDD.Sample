//#define inMemoryDatabase
#define debugLogger
using System;
using System.Collections.Generic;
using System.Linq;

using DotnetCoreInfra.Common;
using DotnetCoreInfra.DataAccess;
using DotnetCoreInfra.DataAccessInterface;
using DotnetCoreInfra.Options;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Ordering.Domain.Entities;
using Ordering.Infrastructure.Context;

namespace DotnetCoreInfra.UnitTests
{
    [TestClass()]
    [TestCategory("dataAccessor")]
    public class DataAccessorTests : TestBase
    {
        private readonly ServiceCollection serviceCollection = new ServiceCollection();

        private IDataAccessor dataAccessor;

        private readonly string testConnectString =
            "Server=.;Database=DDDSample.DataBase.Test;uid=sa;pwd=wpl19950815;";

        private static ILoggerFactory LoggerFactory => new LoggerFactory()
               .AddDebug((categoryName, logLevel) => (logLevel == LogLevel.Information)
               && (categoryName == DbLoggerCategory.Database.Command.Name))
               .AddConsole((categoryName, logLevel) =>
               (logLevel == LogLevel.Information)
               && (categoryName == DbLoggerCategory.Database.Command.Name));

        [TestInitialize]
        public void Init()
        {
            Console.WriteLine($"Configure the connection string if using a real database: {testConnectString}");
            ServiceProvider provider = serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options
#if debugLogger
                    .UseLoggerFactory(LoggerFactory)
                    .EnableSensitiveDataLogging(true)
#endif
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

        [TestMethod]
        public void FindAsync_IdExist_Noraml()
        {
            var entity = MockHelper.MockEntity<OrderEntity>();
            dataAccessor.InsertAsync(entity).GetAwaiter().GetResult();
            var result = dataAccessor.FindAsync<OrderEntity>(entity.Id).GetAwaiter().GetResult();
            Assert.AreEqual(entity.Id, result.Id);
        }

        [TestMethod]
        public void FindAsync_IdNotExist_Null()
        {
            var result = dataAccessor.FindAsync<OrderEntity>(Guid.NewGuid().ToString()).GetAwaiter().GetResult();
            Assert.IsNull(result);
        }

        [TestMethod]
        public void FindAsync_IncludeDeletedIsTrue_Noraml()
        {
            // IsDeleted default value is false
            var entity = MockHelper.MockEntity<OrderEntity>();
            dataAccessor.InsertAsync(entity).GetAwaiter().GetResult();
            entity.IsDeleted = true;
            dataAccessor.UpdateAsync(entity);
            var result = dataAccessor.FindAsync<OrderEntity>(entity.Id, true).GetAwaiter().GetResult();
            Assert.AreEqual(entity.Id, result.Id);
            Assert.IsTrue(entity.IsDeleted.Value);
        }

        [TestMethod]
        public void FindAsync_IncludeDeletedIsFalse_Noraml()
        {
            // IsDeleted default value is false
            var entity = MockHelper.MockEntity<OrderEntity>();
            dataAccessor.InsertAsync(entity).GetAwaiter().GetResult();
            var result = dataAccessor.FindAsync<OrderEntity>(entity.Id).GetAwaiter().GetResult();
            Assert.AreEqual(entity.Id, result.Id);
            Assert.IsFalse(entity.IsDeleted.Value);
        }

        [TestMethod]
        public void InsertAsync_Normal()
        {
            var entity = MockHelper.MockEntity<OrderEntity>();
            var result = dataAccessor.InsertAsync(entity).GetAwaiter().GetResult();
            Assert.AreEqual(entity.Id, result.Id);
            Assert.IsFalse(entity.IsDeleted.Value);
        }

        [TestMethod]
        public void GetListAsync_Normal()
        {
            var entitys = MockHelper.MockDataList<OrderEntity>();
            dataAccessor.BatchInsertAsync(entitys).GetAwaiter().GetResult();
            var results = dataAccessor.GetListAsync<OrderEntity>().GetAwaiter().GetResult();
            Assert.AreEqual(entitys.Count, results.Select(P => P.Id).Intersect(entitys.Select(P => P.Id)).Count());
        }
    }
}