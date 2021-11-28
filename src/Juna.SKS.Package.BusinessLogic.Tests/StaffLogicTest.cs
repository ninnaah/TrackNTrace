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

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class StaffLogicTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IHopRepository> mockHopRepo;
        Mock<ILogger<StaffLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();

            List<DataAccess.Entities.HopArrival> visitedHops = new List<DataAccess.Entities.HopArrival>()
            {
                new DataAccess.Entities.HopArrival(1, "ABCD1234", "A description", DateTime.Now),
                new DataAccess.Entities.HopArrival(2, "DBCA1234", "A description", DateTime.Now)
            };


            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = visitedHops)
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum)
                .Build();
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            mockHopRepo = new Mock<IHopRepository>();
            var returnHopArrival = Builder<DataAccess.Entities.HopArrival>.CreateNew()
                .With(p => p.Id = 1)
                .With(p => p.Code = "ABCD1234")
                .With(p => p.Description = "Hauptlager 27-12")
                .With(p => p.DateTime = Builder<DateTime>.CreateNew().Build())
                .Build();
            mockHopRepo.Setup(m => m.GetSingleHopArrivalByCode(It.IsAny<string>()))
                .Returns(returnHopArrival);

            mockLogger = new Mock<ILogger<StaffLogic>>();
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_ReturnTrue()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);

            Assert.IsTrue(testResult);
        }


        [Test]
        public void ReportParcelDelivery_InvalidTrackingId_ReturnFalse()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string invalidTrackingId = "12";

            try
            {
                var testResult = staff.ReportParcelDelivery(invalidTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void ReportParcelHop_ValidTrackingId_ReturnTrue()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);

            Assert.IsTrue(testResult);
        }


        [Test]
        public void ReportParcelHop_InvalidTrackingId_ReturnFalse()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string invalidTrackingId = "12";
            string validCode = "ABCD1234";

            try
            {
                var testResult = staff.ReportParcelHop(invalidTrackingId, validCode);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }
        [Test]
        public void ReportParcelHop_ValidCode_ReturnTrue()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);

            Assert.IsTrue(testResult);
        }


        [Test]
        public void ReportParcelHop_InvalidCode_ReturnFalse()
        {
            IStaffLogic staff = new StaffLogic(mockParcelRepo.Object, mockHopRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string invalidCode = "12";

            try
            {
                var testResult = staff.ReportParcelHop(validTrackingId, invalidCode);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

    }
}
