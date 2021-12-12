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
    public class RecipientLogicTest
    {
        Mock<IParcelRepository> mockRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<RecipientLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IParcelRepository>();

            mockMapper = new Mock<IMapper>();
            mockMapper.Setup(m => m.Map<Parcel>(It.IsAny<DataAccess.Entities.Parcel>())).Returns(new Parcel());

            mockLogger = new Mock<ILogger<RecipientLogic>>();
        }

        [Test]
        public void TrackParcel_ValidTrackingId_ReturnParcel()
        {
            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            mockRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IRecipientLogic recipient= new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Parcel>(testResult);
        }

        [Test]
        public void TrackParcel_InvalidTrackingId_ThrowValidatorException()
        {
            IRecipientLogic recipient = new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string invalidTrackingId = "12";

            try
            {
                var testResult = recipient.TrackParcel(invalidTrackingId);
                Assert.Fail();
            }
            catch (ValidatorException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TrackParcel_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new DataNotFoundException(null, null));

            IRecipientLogic recipient = new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                var testResult = recipient.TrackParcel(validTrackingId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }

        }

        [Test]
        public void TrackParcel_DataException_ThrowLogicException()
        {
            mockRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
               .Throws(new DataException(null, null));

            IRecipientLogic recipient = new RecipientLogic(mockRepo.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            try
            {
                var testResult = recipient.TrackParcel(validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }

        }

    }
}
