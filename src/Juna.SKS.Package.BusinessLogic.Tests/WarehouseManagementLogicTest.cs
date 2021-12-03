using System;
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
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;

namespace Juna.SKS.Package.BusinessLogic.Tests
{
    public class WarehouseManageLogicTest
    {
        Mock<IHopRepository> mockRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<WarehouseManagementLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IHopRepository>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<WarehouseManagementLogic>>();
        }

        [Test]
        public void ExportWarehouses_NoException_ReturnWarehouse()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Warehouse>(It.IsAny<DataAccess.Entities.Warehouse>())).Returns(new BusinessLogic.Entities.Warehouse());

            var returnWarehouse = Builder<DataAccess.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Id = 1)
                .With(p => p.Level = 1)
                .With(p => p.Parent = Builder<DataAccess.Entities.WarehouseNextHops>.CreateNew().Build())
                .With(p => p.HopType = "Truck")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.ProcessingDelayMins = 3)
                .With(p => p.LocationName = "Wien")
                .With(p => p.LocationCoordinates = Builder<DataAccess.Entities.GeoCoordinate>.CreateNew().Build())
                .With(p => p.NextHops = Builder<DataAccess.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            mockRepo.Setup(m => m.GetWarehouseHierarchy())
                .Returns(returnWarehouse);

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            var testResult = warehouseManagement.ExportWarehouse();

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Warehouse>(testResult);
        }

        [Test]
        public void ExportWarehouses_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockRepo.Setup(m => m.GetWarehouseHierarchy())
                .Throws(new DataNotFoundException(null, null));

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            try
            {
                var testResult = warehouseManagement.ExportWarehouse();
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void ExportWarehouses_DataException_ThrowLogicException()
        {
            mockRepo.Setup(m => m.GetWarehouseHierarchy())
                .Throws(new DataException(null, null));

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            try
            {
                var testResult = warehouseManagement.ExportWarehouse();
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }





        [Test]
        public void GetHop_ValidCode_ReturnWarehouse()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Hop>(It.IsAny<DataAccess.Entities.Hop>())).Returns(new BusinessLogic.Entities.Hop());

            var returnWarehouse = Builder<DataAccess.Entities.Hop>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Id = 1)
                .With(p => p.HopType = "Truck")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.ProcessingDelayMins = 3)
                .With(p => p.LocationName = "Wien")
                .With(p => p.LocationCoordinates = Builder<DataAccess.Entities.GeoCoordinate>.CreateNew().Build())
                .Build();
            mockRepo.Setup(m => m.GetSingleHopByCode(It.IsAny<string>()))
                .Returns(returnWarehouse);

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            var testResult = warehouseManagement.GetHop(validCode);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Hop>(testResult);
        }


        [Test]
        public void GetHop_InvalidCode_ThrowValidatorException()
        {
            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "12";

            try
            {
                var testResult = warehouseManagement.GetHop(validCode);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void GetHop_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockRepo.Setup(m => m.GetSingleHopByCode(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            try
            {
                var testResult = warehouseManagement.GetHop(validCode);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void GetHop_DataException_ThrowLogicException()
        {
            mockRepo.Setup(m => m.GetSingleHopByCode(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validCode = "ABCD1234";

            try
            {
                var testResult = warehouseManagement.GetHop(validCode);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }










        [Test]
        public void ImportWarehouses_ValidWarehouse_ReturnTrue()
        {
            mockMapper.Setup(m => m.Map<DataAccess.Entities.Warehouse>(It.IsAny<BusinessLogic.Entities.Warehouse>())).Returns(new DataAccess.Entities.Warehouse());

            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Hop>()))
                .Returns(1);

            var validWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            var testResult = warehouseManagement.ImportWarehouse(validWarehouse);

            Assert.IsTrue(testResult);
        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseDescription_ThrowValidatorException()
        {
            var invalidWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "..,")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            try
            {
                var testResult = warehouseManagement.ImportWarehouse(invalidWarehouse);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ImportWarehouses_InvalidWarehouseNextHopsIsNull_ThrowValidatorException()
        {
            var invalidWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = null)
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            try
            {
                var testResult = warehouseManagement.ImportWarehouse(invalidWarehouse);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ImportWarehouses_DataExceptionCreate_ThrowLogicException()
        {
            mockMapper.Setup(m => m.Map<DataAccess.Entities.Warehouse>(It.IsAny<BusinessLogic.Entities.Warehouse>())).Returns(new DataAccess.Entities.Warehouse());

            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Hop>()))
                .Throws(new DataException(null, null));

            var validWarehouse = Builder<BusinessLogic.Entities.Warehouse>.CreateNew()
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Level = 1)
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.NextHops = Builder<BusinessLogic.Entities.WarehouseNextHops>.CreateListOfSize(3).Build().ToList())
                .Build();

            IWarehouseManagementLogic warehouseManagement = new WarehouseManagementLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            try
            {
                var testResult = warehouseManagement.ImportWarehouse(validWarehouse);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

    }
}
