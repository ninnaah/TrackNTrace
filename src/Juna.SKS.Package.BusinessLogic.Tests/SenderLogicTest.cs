using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic;
using Moq;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using NetTopologySuite.Geometries;

namespace Juna.SKS.Package.BusinessLogic.Tests
{
    public class SenderLogicTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IHopRepository> mockHopRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<SenderLogic>> mockLogger;
        Mock<IGeoEncodingAgent> mockAgent;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockHopRepo = new Mock<IHopRepository>();

            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<GeoCoordinate>(It.IsAny<DataAccess.Entities.GeoCoordinate>())).Returns(new GeoCoordinate( 5, 5));
            mockMapper.Setup(m => m.Map<DataAccess.Entities.Parcel>(It.IsAny<Parcel>())).Returns(new DataAccess.Entities.Parcel());

            mockLogger = new Mock<ILogger<SenderLogic>>();

            mockAgent = new Mock<IGeoEncodingAgent>();

        }

        [Test]
        public void SubmitParcel_ValidParcel_ReturnTrackingId()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 5,5));
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
               .Returns(1);

            Coordinate[] coords = new Coordinate[]{
                new Coordinate(0,0),
                new Coordinate(0,10),
                new Coordinate(10,10),
                new Coordinate(10,0),
                new Coordinate(0,0) 
			};
            GeometryFactory geometryFactory = new GeometryFactory();
            Polygon poly = geometryFactory.CreatePolygon(coords);

            var returnTrucks = new List<DataAccess.Entities.Hop>()
            {
                new DataAccess.Entities.Truck(1, poly, "something", "Truck", "ABCD12345", "a description", 1, "Location1", null),
                new DataAccess.Entities.Truck(2, poly, "something", "Truck", "ABCD12346", "another description", 2, "Location2", null),
                new DataAccess.Entities.Truck(3, poly, "something", "Truck", "ABCD12347", "still a description", 3, "Location3", null)
            };

            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Returns(returnTrucks);

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.AreEqual(validParcel.TrackingId, testResult);
        }

        [Test]
        public void SubmitParcel_InvalidParcelInvalidTrackingId_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "12")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(invalidParcel);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_InvalidParcelZeroWeight_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 0)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(invalidParcel);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }


        }

        [Test]
        public void SubmitParcel_InvalidParcelRecipientIsNull_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = null)
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(invalidParcel);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }


        }

        [Test]
        public void SubmitParcel_InvalidParcelSenderIsNull_ThrowValidatorException()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = null)
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(invalidParcel);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }


        }
        

        [Test]
        public void SubmitParcel_DataNotFoundExceptionEncodeAdress_ThrowLogicDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new DataNotFoundException(null, null));
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new DataNotFoundException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_ExceptionEncodeAdress_ThrowLogicException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new DataNotFoundException(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_DataNotFoundExceptionGetHopsByHopType_ThrowLogicDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
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

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_DataExceptionGetHopsByHopType_ThrowLogicDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
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

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_ExceptionGetHopsByHopType_ThrowLogicDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 8.354502800000091, 54.912746486000046));
            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Throws(new Exception(null, null));

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_DataExceptionCreate_ThrowLogicDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 5, 5));
           
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
                 .Throws(new DataException(null, null));

            Coordinate[] coords = new Coordinate[]{
                new Coordinate(0,0),
                new Coordinate(0,10),
                new Coordinate(10,10),
                new Coordinate(10,0),
                new Coordinate(0,0)
            };
            GeometryFactory geometryFactory = new GeometryFactory();
            Polygon poly = geometryFactory.CreatePolygon(coords);

            var returnTrucks = new List<DataAccess.Entities.Hop>()
            {
                new DataAccess.Entities.Truck(1, poly, "something", "Truck", "ABCD12345", "a description", 1, "Location1", null),
                new DataAccess.Entities.Truck(2, poly, "something", "Truck", "ABCD12346", "another description", 2, "Location2", null),
                new DataAccess.Entities.Truck(3, poly, "something", "Truck", "ABCD12347", "still a description", 3, "Location3", null)
            };

            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Returns(returnTrucks);

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void SubmitParcel_ExceptionCreate_ThrowLogicDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());
            mockAgent.Setup(m => m.EncodeAddress(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new DataAccess.Entities.GeoCoordinate(1, 5, 5));
            
            mockParcelRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
                 .Throws(new Exception(null, null));


            Coordinate[] coords = new Coordinate[]{
                new Coordinate(0,0),
                new Coordinate(0,10),
                new Coordinate(10,10),
                new Coordinate(10,0),
                new Coordinate(0,0)
            };
            GeometryFactory geometryFactory = new GeometryFactory();
            Polygon poly = geometryFactory.CreatePolygon(coords);

            var returnTrucks = new List<DataAccess.Entities.Hop>()
            {
                new DataAccess.Entities.Truck(1, poly, "something", "Truck", "ABCD12345", "a description", 1, "Location1", null),
                new DataAccess.Entities.Truck(2, poly, "something", "Truck", "ABCD12346", "another description", 2, "Location2", null),
                new DataAccess.Entities.Truck(3, poly, "something", "Truck", "ABCD12347", "still a description", 3, "Location3", null)
            };

            mockHopRepo.Setup(m => m.GetHopsByHopType(It.IsAny<string>()))
                .Returns(returnTrucks);

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().With(x => x.Country = "Österreich").Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);

            try
            {
                var testResult = sender.SubmitParcel(validParcel);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void GenerateTrackingId_TrackingIdUniqueException_ReturnTrackingId()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new Exception());

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);
            var trackingId = sender.GenerateTrackingId();

            Assert.IsNotNull(trackingId);
        }

        [Test]
        public void GenerateTrackingId_TrackingIdNotUniqueRetry_ReturnTrackingId()
        {
            mockParcelRepo.SetupSequence(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Returns(It.IsAny<DataAccess.Entities.Parcel>())
               .Throws(new Exception());

            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockParcelRepo.Object, mockHopRepo.Object, mockMapper.Object, mockLogger.Object, mockAgent.Object);
            var trackingId = sender.GenerateTrackingId();

            Assert.IsNotNull(trackingId);

        }

    }
}
