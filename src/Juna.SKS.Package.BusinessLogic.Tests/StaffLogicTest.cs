using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;

namespace Juna.SKS.Package.BusinessLogic.Tests
{
    public class StaffLogicTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IHopRepository> mockHopRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<StaffLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockHopRepo = new Mock<IHopRepository>();

            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<StaffLogic>>();
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_DontThrowException()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Parcel>(It.IsAny<DataAccess.Entities.Parcel>())).Returns(new BusinessLogic.Entities.Parcel());

            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<BusinessLogic.Entities.Parcel>())).Returns(new DataAccess.Entities.Parcel());

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);
            mockParcelRepo.Setup(m => m.Update(It.IsAny<DataAccess.Entities.Parcel>()));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                staff.ReportParcelDelivery(validTrackingId);
                Assert.Pass();
            }
            catch (Exception)
            {
                Assert.Fail();
            }

        }

        [Test]
        public void ReportParcelDelivery_InvalidTrackingId_ThrowValidatorException()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string invalidTrackingId = "12";

            try
            {
                staff.ReportParcelDelivery(invalidTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelDelivery_DataNotFoundExceptionGetParcel_ThrowLogicDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null,null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                staff.ReportParcelDelivery(validTrackingId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelDelivery_DataExceptionGetParcel_ThrowLogicException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                staff.ReportParcelDelivery(validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelDelivery_DataExceptionUpdate_ThrowLogicException()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Parcel>(It.IsAny<DataAccess.Entities.Parcel>())).Returns(new BusinessLogic.Entities.Parcel());

            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<BusinessLogic.Entities.Parcel>())).Returns(new DataAccess.Entities.Parcel());

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);
            mockParcelRepo.Setup(m => m.Update(It.IsAny<DataAccess.Entities.Parcel>())).Throws(new DataException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                staff.ReportParcelDelivery(validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }





        [Test]
        public void ReportParcelHop_ValidTrackingId_DontThrowException()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.HopArrival>(It.IsAny<DataAccess.Entities.HopArrival>())).Returns(new BusinessLogic.Entities.HopArrival());

            var BLreturnParcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = BusinessLogic.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Parcel>(It.IsAny<DataAccess.Entities.Parcel>())).Returns(BLreturnParcel);

            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<BusinessLogic.Entities.Parcel>())).Returns(new DataAccess.Entities.Parcel());


            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            var returnHopArrival = Builder<DataAccess.Entities.HopArrival>.CreateNew()
                .With(p => p.Id = 1)
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.DateTime = Builder<DateTime>.CreateNew().Build())
                .Build();
            mockHopRepo.Setup(m => m.GetSingleHopArrivalByCode(It.IsAny<string>()))
                .Returns(returnHopArrival);

            mockParcelRepo.Setup(m => m.Update(It.IsAny<DataAccess.Entities.Parcel>()));


            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            Assert.DoesNotThrow(() => staff.ReportParcelHop(validTrackingId, validCode));

        }

        [Test]
        public void ReportParcelHop_InvalidTrackingId_ThrowValidatorException()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string invalidTrackingId = "12";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(invalidTrackingId, validCode);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_InvalidCode_ThrowValidatorException()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6"; 
            string invalidCode = "12";

            try
            {
                staff.ReportParcelHop(validTrackingId, invalidCode);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_DataNotFoundExceptionGetParcel_ThrowLogicDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));


            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(validTrackingId, validCode);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_DataExceptionGetParcel_ThrowLogicException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(validTrackingId, validCode);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_DataNotFoundExceptionGetHop_ThrowLogicDataNotFoundException()
        {
            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            mockHopRepo.Setup(m => m.GetSingleHopArrivalByCode(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(validTrackingId, validCode);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_DataExceptionGetHop_ThrowLogicException()
        {
            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            mockHopRepo.Setup(m => m.GetSingleHopArrivalByCode(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(validTrackingId, validCode);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_DataExceptionUpdate_ThrowLogicException()
        {
            var BLreturnParcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = BusinessLogic.Entities.Parcel.StateEnum.InTransportEnum)
                .Build();
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.Parcel>(It.IsAny<DataAccess.Entities.Parcel>())).Returns(BLreturnParcel);

            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<BusinessLogic.Entities.Parcel>())).Returns(new DataAccess.Entities.Parcel());

            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.HopArrival>(It.IsAny<DataAccess.Entities.HopArrival>())).Returns(new BusinessLogic.Entities.HopArrival());

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.Id = 1)
               .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.InTransportEnum)
               .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            var returnHopArrival = Builder<DataAccess.Entities.HopArrival>.CreateNew()
                .With(p => p.Id = 1)
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.DateTime = Builder<DateTime>.CreateNew().Build())
                .Build();
            mockHopRepo.Setup(m => m.GetSingleHopArrivalByCode(It.IsAny<string>()))
                .Returns(returnHopArrival);

            mockParcelRepo.Setup(m => m.Update(It.IsAny<DataAccess.Entities.Parcel>())).Throws(new DataException(null, null));

            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            try
            {
                staff.ReportParcelHop(validTrackingId, validCode);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }





    }
}
