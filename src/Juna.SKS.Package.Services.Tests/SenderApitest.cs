using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.Services.Controllers;
using Moq;
using AutoMapper;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;

namespace Juna.SKS.Package.Services.Tests
{
    public class SenderApiTest
    {
        Mock<ISenderLogic> mockLogic;
        Mock<IMapper> mockMapper;
        Mock<ILogger<SenderApiController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<ISenderLogic>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<SenderApiController>>();
        }

        [Test]
        public void SubmitParcel_ValidParcel_ReturnCode201()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
                .Returns("PYJRB4HZ6");

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(validParcel);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(201, testResultCode.StatusCode);
        }

        [Test]
        public void SubmitParcel_ValidatorExceptionInvalidParcelWeight_ReturnCode400()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
               .Throws(new ValidatorException(null, null, null));

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var invalidParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 0)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(invalidParcel);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }
        [Test]
        public void SubmitParcel_LogicDataNotFoundException_ReturnCode400()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
               .Throws(new LogicDataNotFoundException(null, null, null));

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(validParcel);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

        [Test]
        public void SubmitParcel_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
               .Throws(new LogicException(null, null, null));

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(validParcel);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

    }
}
