﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.BusinessLogic;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class WarehouseManagerLogicTest
    {
        Mock<IHopRepository> mockRepo;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IHopRepository>();
            var returnWarehouse = Builder<DataAccess.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Id = 1)
                .With(p => p.Level = 1)
                .With(p => p.HopType = "Truck")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.ProcessingDelayMins = 3)
                .With(p => p.LocationName = "Wien")
                .With(p => p.LocationCoordinates = Builder<DataAccess.Entities.GeoCoordinate>.CreateNew().Build())
                .With(p => p.NextHops = Builder<DataAccess.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();
            mockRepo.Setup(m => m.GetSingleWarehouseByCode(It.IsAny<string>()))
                .Returns(returnWarehouse);
            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Hop>()))
                .Returns(1);

            mockMapper = new Mock<IMapper>();
        }
        [Test]
        public void ExportWarehouses_ValidWarehouses_ReturnWarehouse()
        {
            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            var testResult = warehouseManagement.ExportWarehouses();

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Warehouse>(testResult);
        }


        /*[Test]
        public void GetWarehouse_ValidCode_ReturnWarehouse()
        {
            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetWarehouse(validCode);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Warehouse>(testResult);
        }*/


        [Test]
        public void GetWarehouse_InvalidCode_ReturnNull()
        {
            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            string validCode = "12";

            var testResult = warehouseManagement.GetWarehouse(validCode);

            Assert.IsNull(testResult);
        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_ReturnTrue()
        {
            var validWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            var testResult = warehouseManagement.ImportWarehouses(validWarehouse);

            Assert.IsTrue(testResult);
        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseInvalidDescription_ReturnFalse()
        {
            var invalidWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "..,")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            var testResult = warehouseManagement.ImportWarehouses(invalidWarehouse);

            Assert.IsFalse(testResult);
        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseNextHopsIsNull_ReturnFalse()
        {
            var invalidWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = null)
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object);

            var testResult = warehouseManagement.ImportWarehouses(invalidWarehouse);

            Assert.IsFalse(testResult);
        }

    }
}
