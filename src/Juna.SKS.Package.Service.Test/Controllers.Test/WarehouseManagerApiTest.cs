﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.Services.Controllers;
using Moq;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class WarehouseManagerApiTest
    {
        Mock<IWarehouseManagementLogic> mockLogic;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IWarehouseManagementLogic>();
            mockMapper = new Mock<IMapper>();
        }

        [Test]
        public void ExportWarehouses_ValidWarehouses_ReturnCode200()
        {
            var returnWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops> .CreateListOfSize(3).Build().ToList())
                .Build();

            mockLogic.Setup(m => m.ExportWarehouses())
                .Returns(returnWarehouse);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object);

            var testResult = warehouseManagement.ExportWarehouses();
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void GetWarehouse_ValidCode_ReturnCode200()
        {
            var returnWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            mockLogic.Setup(m => m.GetWarehouse(It.IsAny<string>()))
                .Returns(returnWarehouse);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetWarehouse(validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void GetWarehouse_InvalidCode_ReturnCode400()
        {
            mockLogic.Setup(m => m.GetWarehouse(It.IsAny<string>()))
                .Returns(value : null);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object);

            string invalidCode = "12";

            var testResult = warehouseManagement.GetWarehouse(invalidCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

        [Test]
        public void ImportWarehouses_ValidWarehouse_ReturnCode200()
        {
            mockLogic.Setup(m => m.ImportWarehouses(It.IsAny<BusinessLogic.Entities.Warehouse>()))
                .Returns(true);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object);

            var validWarehouse = Builder<DTOs.Models.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<DTOs.Models.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            var testResult = warehouseManagement.ImportWarehouses(validWarehouse);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);

        }


        [Test]
        public void ImportWarehouses_InvalidWarehouse_ReturnCode400()
        {
            mockLogic.Setup(m => m.ImportWarehouses(It.IsAny<BusinessLogic.Entities.Warehouse>()))
                 .Returns(false);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object);

            var invalidWarehouse = Builder<DTOs.Models.Warehouse>.CreateNew()
                .With(p => p.Code = "12")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<DTOs.Models.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            var testResult = warehouseManagement.ImportWarehouses(invalidWarehouse);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

    }
}
