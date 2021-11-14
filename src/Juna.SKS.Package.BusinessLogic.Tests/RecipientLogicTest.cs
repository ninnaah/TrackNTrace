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

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class RecipientLogicTest
    {
        Mock<IParcelRepository> mockRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<RecipientLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IParcelRepository>();

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Id = 1)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.State = DataAccess.Entities.Parcel.StateEnum.DeliveredEnum)
                .Build();

            mockRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<RecipientLogic>>();
        }
        /*[Test]
        public void TrackParcel_ValidTrackingId_ReturnParcel()
        {
            IRecipientLogic recipient= new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Parcel>(testResult);
        }*/

        [Test]
        public void TrackParcel_InvalidTrackingId_ReturnNull()
        {
            IRecipientLogic recipient = new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "12";

            var testResult = recipient.TrackParcel(validTrackingId);

            Assert.IsNull(testResult);
        }

    }
}
