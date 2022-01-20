using FizzWare.NBuilder;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Tests
{
    public class SqlWebhookRepositoryTest
    {
        Mock<ILogger<SqlWebhookRepository>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<SqlWebhookRepository>>();
        }

        [Test, Order(1)]
        public void Create_ValidWebhook_ReturnIdAndCreateWebhook()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                context.WebhookResponses.Add(new WebhookResponse { Id = 1 });
                context.WebhookResponses.Add(new WebhookResponse { Id = 2 });
                context.WebhookResponses.Add(new WebhookResponse { Id = 3 });

                context.SaveChanges();
            }

            using (var context = new DBContext(options))
            {
                IWebhookRepository repo = new SqlWebhookRepository(context, mockLogger.Object);
                var validHook = Builder<WebhookResponse>.CreateNew()
                 .With(p => p.Id = 4)
                 .Build();

                long id = repo.Create(validHook);

                Assert.NotNull(id);
                Assert.NotNull(context.WebhookResponses);
                Assert.AreEqual(context.WebhookResponses.Count(), id);

            }

        }
        /*[Test, Order(2)]
        public void Create_SqlException_ThrowDataException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            using (var context = new DBContext(options))
            {
                IWebhookRepository repo = new SqlWebhookRepository(context, mockLogger.Object);
                var invalidHook = Builder<WebhookResponse>.CreateNew()
                 .With(p => p.Id = 4)
                 .Build();

                int expectedCountHops = 4;

                try
                {
                    long id = repo.Create(invalidHook);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.WebhookResponses.Count(), expectedCountHops);
                }
            }

        }
        [Test, Order(3)]
        public void Create_Exception_ThrowDataException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            using (var context = new DBContext(options))
            {
                IWebhookRepository repo = new SqlWebhookRepository(context, mockLogger.Object);
                var invalidHook = Builder<WebhookResponse>.CreateNew()
                 .With(p => p.Id = 4)
                 .Build();

                int expectedCountHops = 4;

                try
                {
                    long id = repo.Create(invalidHook);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.WebhookResponses.Count(), expectedCountHops);
                }
            }

        }*/
    }
}
