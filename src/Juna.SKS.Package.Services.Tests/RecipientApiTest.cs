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
    public class RecipientApiTest
    {
        Mock<IRecipientLogic> mockLogic;
        Mock<IMapper> mockMapper;
        Mock<ILogger<RecipientApiController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IRecipientLogic>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<RecipientApiController>>();
        }

        [Test]
        public void TrackParcel_ValidTrackingId_ReturnCode200()
        {
            var returnParcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .Build();

            mockLogic.Setup(m => m.TrackParcel(It.IsAny<string>()))
                .Returns(returnParcel);

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void TrackParcel_ValidatorExceptionInvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.TrackParcel(It.IsAny<string>()))
                .Throws(new ValidatorException(null, null, null));

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var invalidTrackingId = "12";

            var testResult = recipient.TrackParcel(invalidTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }

        [Test]
        public void TrackParcel_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.TrackParcel(It.IsAny<string>()))
                .Throws(new LogicDataNotFoundException(null, null, null));

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }

        [Test]
        public void TrackParcel_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.TrackParcel(It.IsAny<string>()))
                .Throws(new LogicException(null, null, null));

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }


    }
}
