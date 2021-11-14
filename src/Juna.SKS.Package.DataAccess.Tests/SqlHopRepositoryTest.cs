using FizzWare.NBuilder;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Sql;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Tests
{
    /*public class SqlHopRepositoryTest
    {
        Mock<DBContext> mockDBContext;

        List<Hop> hops;
        List<GeoCoordinate> geoCoordinates;
        List<Warehouse> warehouses; 
        List<Truck> trucks;
        List<Transferwarehouse> transferwarehouses;
        List<WarehouseNextHops> warehouseNextHops;

        [SetUp]
        public void Setup()
        {
            mockDBContext = new Mock<DBContext>();


            hops = new List<Hop>();
            geoCoordinates = new List<GeoCoordinate>();
            warehouseNextHops = new List<WarehouseNextHops>();

            mockDBContext.Setup(p => p.Hops).Returns(DbContextMock.GetQueryableMockDbSet<Hop>(hops));
            mockDBContext.Setup(p => p.GeoCoordinates).Returns(DbContextMock.GetQueryableMockDbSet<GeoCoordinate>(geoCoordinates));
            mockDBContext.Setup(p => p.Warehouses).Returns(DbContextMock.GetQueryableMockDbSet<Warehouse>(warehouses));
            mockDBContext.Setup(p => p.Trucks).Returns(DbContextMock.GetQueryableMockDbSet<Truck>(trucks));
            mockDBContext.Setup(p => p.Transferwarehouses).Returns(DbContextMock.GetQueryableMockDbSet<Transferwarehouse>(transferwarehouses)); 
            mockDBContext.Setup(p => p.WarehouseNextHops).Returns(DbContextMock.GetQueryableMockDbSet<WarehouseNextHops>(warehouseNextHops));

            mockDBContext.Setup(p => p.SaveChanges()).Returns(1);

        }

        [Test]
        public void Create_ValidHop_ReturnIdAndCreateHop()
        {

            SqlHopRepository repo = new SqlHopRepository(mockDBContext.Object);

            var validHop = Builder<Hop>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.HopType = "truck")
                .With(p => p.ProcessingDelayMins = 2)
                .With(p => p.Id = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .Build();

            int id = repo.Create(validHop);

            Assert.NotNull(id);
            Assert.NotNull(hops);
            Assert.AreEqual(hops.First().Id, id);
        }

        [Test]
        public void Update_ValidHop_UpdateHop()
        {

        }

        [Test]
        public void Update_InvalidHop_DontUpdateHop()
        {

        }

        [Test]
        public void Delete_ValidId_DeleteHop()
        {

        }

        [Test]
        public void Delete_InvalidId_DontDeleteHop()
        {

        }

        [Test]
        public void GetSingleHopArrivalByCode_ValidCode_ReturnHopArrival()
        {

        }

        [Test]
        public void GetSingleHopArrivalByCode_InvalidCode_ReturnNull()
        {

        }

        [Test]
        public void GetSingleWarehouseByCode_ValidCode_ReturnWarehouse()
        {

        }

        [Test]
        public void GetSingleWarehouseByCode_InvalidCode_ReturnNull()
        {

        }

        [Test]
        public void GetSingleHopById_ValidId_ReturnWarehouse()
        {

        }

        [Test]
        public void GetSingleHopById_InvalidId_ReturnNull()
        {

        }


    }*/
}
