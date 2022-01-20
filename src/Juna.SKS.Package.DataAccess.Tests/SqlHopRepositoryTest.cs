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
    public class SqlHopRepositoryTest
    {
        Mock<ILogger<SqlHopRepository>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogger = new Mock<ILogger<SqlHopRepository>>();
        }


        [Test, Order(1)]
        public void Create_ValidHop_ReturnIdAndCreateHop()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                context.Warehouses.Add(new Warehouse { Id = 1, HopType = "Warehouse", Code="ABCD", Description ="Description", ProcessingDelayMins = 2, LocationName="Wien", LocationCoordinates = new(), Parent = new()});
                context.Warehouses.Add(new Warehouse { Id = 2, Level = 0, HopType = "Warehouse", Code = "ABCD", Description = "Description", ProcessingDelayMins = 2, LocationName = "Wien", LocationCoordinates = new(), Parent = new(), NextHops = new() });

                context.Trucks.Add(new Truck { Id = 3, HopType = "Truck", Code = "DCBA", Description = "Description", ProcessingDelayMins = 2, LocationName = "Wien", LocationCoordinates = new(), Parent = new() });
                context.Trucks.Add(new Truck { Id = 4, HopType = "Truck", Code = "BACD", Description = "Description", ProcessingDelayMins = 2, LocationName = "Wien", LocationCoordinates = new(), Parent = new() });

                context.SaveChanges();
            }

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);
                var validHop = Builder<Warehouse>.CreateNew()
                 .With(p => p.Id = 5)
                 .With(p => p.HopType = "Warehouse")
                 .Build();

                int id = repo.Create(validHop);

                Assert.NotNull(id);
                Assert.NotNull(context.Hops);
                Assert.AreEqual(context.Hops.Count(), id);

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
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);
                var invalidHop = Builder<Warehouse>.CreateNew()
                 .With(p => p.Id = 1)
                 .Build();

                int expectedCountHops = 5;

                try
                {
                    int id = repo.Create(invalidHop);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.Hops.Count(), expectedCountHops);
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
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);
                var invalidHop = Builder<Warehouse>.CreateNew()
                 .With(p => p.Id = 1)
                 .Build();

                int expectedCountHops = 5;

                try
                {
                    int id = repo.Create(invalidHop);
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.AreEqual(context.Hops.Count(), expectedCountHops);
                }
            }

        }




        [Test, Order(4)]
        public void GetHopsByHopType_NoException_ReturnHops()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);

                IEnumerable<Hop> hops = repo.GetHopsByHopType("Truck");

                Assert.NotNull(hops);
                Assert.AreEqual(hops.Count(), 2);
            }
        }
        /*[Test, Order(5)]
        public void GetHopsByHopType_DataNotFound_ThrowDataNotFoundException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);

                try
                {
                    IEnumerable<Hop> hops = repo.GetHopsByHopType("TransferWarehouse");
                    Assert.Fail();
                }
                catch (DataNotFoundException)
                {
                    Assert.Pass();
                }
            }
        }*/





        [Test, Order(6)]
        public void GetSingleHopByCode_NoException_ReturnHop()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);

                Hop hop = repo.GetSingleHopByCode("BACD");

                Assert.NotNull(hop);
                Assert.AreEqual(hop.Code, "BACD");
            }
        }
        [Test, Order(7)]
        public void GetSingleHopByCode_DataNotFound_ThrowDataException()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);

                try
                {
                    Hop hop = repo.GetSingleHopByCode("123");
                    Assert.Fail();
                }
                catch (DataException)
                {
                    Assert.Pass();
                }
            }
        }

        [Test, Order(8)]
        public void GetWarehouseHierarchy_NoException_ReturnWarehouse()
        {
            var options = new DbContextOptionsBuilder<DBContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new DBContext(options))
            {
                IHopRepository repo = new SqlHopRepository(context, mockLogger.Object);

                Warehouse warehouse = repo.GetWarehouseHierarchy();

                Assert.NotNull(warehouse);
            }
        }
    }
}
