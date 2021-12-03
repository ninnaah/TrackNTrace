using System;
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
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;

namespace Juna.SKS.Package.Services.Tests
{
    public class WarehouseManagementApiTest
    {
        Mock<IWarehouseManagementLogic> mockLogic;
        Mock<IMapper> mockMapper;
        Mock<ILogger<WarehouseManagementApiController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IWarehouseManagementLogic>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<WarehouseManagementApiController>>();
        }

        [Test]
        public void ExportWarehouse_ValidWarehouse_ReturnCode200()
        {
            var returnWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops> .CreateListOfSize(3).Build().ToList())
                .Build();

            mockLogic.Setup(m => m.ExportWarehouse())
                .Returns(returnWarehouse);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var testResult = warehouseManagement.ExportWarehouses();
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void ExportWarehouse_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.ExportWarehouse())
                 .Throws(new LogicDataNotFoundException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var testResult = warehouseManagement.ExportWarehouses();
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }

        [Test]
        public void ExportWarehouse_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.ExportWarehouse())
                 .Throws(new LogicException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var testResult = warehouseManagement.ExportWarehouses();
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }








        [Test]
        public void GetWarehouse_ValidWarehouseCode_ReturnCode200()
        {
            var returnWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            mockLogic.Setup(m => m.GetHop(It.IsAny<string>()))
                .Returns(returnWarehouse);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetWarehouse(validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void GetWarehouse_ValidatorExceptionInvalidWarehouseCode_ReturnCode400()
        {
            mockLogic.Setup(m => m.GetHop(It.IsAny<string>()))
                .Throws(new ValidatorException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string invalidCode = "12";

            var testResult = warehouseManagement.GetWarehouse(invalidCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

        [Test]
        public void GetWarehouse_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.GetHop(It.IsAny<string>()))
                .Throws(new LogicDataNotFoundException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetWarehouse(validCode);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);

        }
        [Test]
        public void GetWarehouse_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.GetHop(It.IsAny<string>()))
                .Throws(new LogicException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetWarehouse(validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }







        [Test]
        public void ImportWarehouse_ValidWarehouseCode_ReturnCode200()
        {
            mockLogic.Setup(m => m.ImportWarehouse(It.IsAny<BusinessLogic.Entities.Warehouse>()))
                .Returns(true);

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validWarehouse = Builder<DTOs.Models.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<DTOs.Models.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            var testResult = warehouseManagement.ImportWarehouses(validWarehouse);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);

        }


        [Test]
        public void ImportWarehouse_ValidatorExceptionInvalidWarehouseCode_ReturnCode400()
        {
            mockLogic.Setup(m => m.ImportWarehouse(It.IsAny<BusinessLogic.Entities.Warehouse>()))
                 .Throws(new ValidatorException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

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

        [Test]
        public void ImportWarehouse_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.ImportWarehouse(It.IsAny<BusinessLogic.Entities.Warehouse>()))
                 .Throws(new LogicException(null, null, null));

            WarehouseManagementApiController warehouseManagement = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var invalidWarehouse = Builder<DTOs.Models.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
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
