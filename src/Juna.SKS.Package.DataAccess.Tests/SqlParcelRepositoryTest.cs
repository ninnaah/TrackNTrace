using FizzWare.NBuilder;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Tests
{
    public class SqlParcelRepositoryTest
    {
        Mock<ILogger<SqlParcelRepository>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<SqlParcelRepository>>();
        }

        [Test, Order(1)]
        public void Create_ValidParcel_ReturnIdAndCreateParcel()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                context.Parcels.Add(new Parcel { Id = 1 });
                context.Parcels.Add(new Parcel { Id = 2 });
                context.Parcels.Add(new Parcel { Id = 3 });

                context.SaveChanges();
            }

            using (var context = new DBContext(options))
            {
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);
                var validParcelp = Builder<Parcel>.CreateNew()
                 .With(p => p.Id = 4)
                 .With(p => p.TrackingId = "ABCD12345")
                 .Build();

                int id = repo.Create(validParcelp);

                Assert.NotNull(id);
                Assert.NotNull(context.Parcels);
                Assert.AreEqual(context.Parcels.Count(), id);

            }

        }
        [Test, Order(2)]
        public void Create_SqlException_ThrowDataException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            using (var context = new DBContext(options))
            {
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);
                var invalidParcel = Builder<Parcel>.CreateNew()
                 .With(p => p.Id = 4)
                 .Build();

                int expectedCountHops = 4;

                try
                {
                    int id = repo.Create(invalidParcel);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.Parcels.Count(), expectedCountHops);
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
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);
                var invalidParcel = Builder<Parcel>.CreateNew()
                 .With(p => p.Id = 4)
                 .Build();

                int expectedCountHops = 4;

                try
                {
                    int id = repo.Create(invalidParcel);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.Parcels.Count(), expectedCountHops);
                }
            }

        }


        /*[Test, Order(4)]
        public void Update_Valid_NoException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;


            using (var context = new DBContext(options))
            {
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);
                var validParcel = Builder<Parcel>.CreateNew()
                 .With(p => p.Id = 4)
                 .With(p => p.TrackingId = "ABCD12345")
                 .Build();


                repo.Update(validParcel);

                Assert.NotNull(context.Parcels);
                Assert.AreEqual(context.Parcels.Count(), 4);
            }

        }*/

        [Test, Order(5)]
        public void GetSingleParcelByTrackingId_NoException_ReturnParcel()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);

                Parcel parcel = repo.GetSingleParcelByTrackingId("ABCD12345");

                Assert.NotNull(parcel);
                Assert.AreEqual(parcel.TrackingId, "ABCD12345");
            }
        }
        /*[Test, Order(6)]
        public void GetSingleParcelByTrackingId_DataNotFound_ThrowDataException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IParcelRepository repo = new SqlParcelRepository(context, mockLogger.Object);

                try
                {
                    Parcel parcel = repo.GetSingleParcelByTrackingId("ABCD12345");
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.Pass();
                }
            }
        }*/


    }
}
