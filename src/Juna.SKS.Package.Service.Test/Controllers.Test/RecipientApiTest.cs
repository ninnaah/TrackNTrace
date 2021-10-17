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

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class RecipientApiTest
    {
        Mock<IRecipientLogic> mockLogic;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IRecipientLogic>();
            mockMapper = new Mock<IMapper>();
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

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object);

            var validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void TrackParcel_InvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.TrackParcel(It.IsAny<string>()))
                .Returns(value: null);

            RecipientApiController recipient = new(mockLogic.Object, mockMapper.Object);

            var invalidTrackingId = "12";

            var testResult = recipient.TrackParcel(invalidTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }


    }
}
