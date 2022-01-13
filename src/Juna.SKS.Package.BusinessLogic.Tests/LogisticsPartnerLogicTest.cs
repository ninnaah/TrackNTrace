using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using FizzWare.NBuilder;
using Moq;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;

namespace Juna.SKS.Package.BusinessLogic.Tests
{
    public class LogisticsPartnerLogicTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IHopRepository> mockHopRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<LogisticsPartnerLogic>> mockLogger;
        Mock<IGeoEncodingAgent> mockAgent;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockHopRepo = new Mock<IHopRepository>();

            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<GeoCoordinate>(It.IsAny<DataAccess.Entities.GeoCoordinate>())).Returns(new GeoCoordinate());
            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<Parcel>())).Returns(new DataAccess.Entities.Parcel());

            mockLogger = new Mock<ILogger<LogisticsPartnerLogic>>();

            mockAgent = new Mock<IGeoEncodingAgent>();
        }

       /* [Test]
        public void TransitionParcel_ValidParcel_ReturnTrackingId()
        {
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 8.354502800000091, 54.912746486000046));
            var returnTrucks = Builder<DataAccess.Entities.Hop>.CreateListOfSize(3).Build().ToList();
            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Returns(returnTrucks);
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
                .Returns(1);

            var validParcel = Builder<Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
                .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Deutschland").Build())
                .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);

            Assert.AreEqual(validTrackingId, testResult);
        }*/

        [Test]
        public void TransitionParcel_InvalidParcelZeroWeight_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
                .With(p => p.Weight = 0)
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
                .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_InvalidParcelRecipientIsNull_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = null)
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_InvalidParcelSenderIsNull_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = null)
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_InvalidParcelInvalidTrackingId_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "12")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string invalidTrackingId = "12";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(invalidParcel, invalidTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_DataNotFoundExceptionEncodeAdress_ThrowLogicDataNotFoundException()
        {
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new DataNotFoundException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_DataNotFoundExceptionGetHopsByHopType_ThrowLogicDataNotFoundException()
        {
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 8.354502800000091, 54.912746486000046));
            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TransitionParcel_DataExceptionGetHopsByHopType_ThrowLogicException()
        {
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 8.354502800000091, 54.912746486000046));
            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        /*[Test]
        public void TransitionParcel_DataExceptionCreate_ThrowLogicException()
        {
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 8.354502800000091, 54.912746486000046));
            var returnTrucks = Builder<DataAccess.Entities.Hop>.CreateListOfSize(3).Build().ToList();
            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Returns(returnTrucks);
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
                 .Throws(new DataException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Deutschland").Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }*/
    }
}
