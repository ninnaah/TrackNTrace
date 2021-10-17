﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.Services.Controllers;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Entities;
using AutoMapper;
using FizzWare.NBuilder;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class LogisticsPartnerApiTest
    {
        Mock<ILogisticsPartnerLogic> mockLogic;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<ILogisticsPartnerLogic>();
            mockMapper = new Mock<IMapper>();
        }

        [Test]
        public void TransitionParcel_ValidParcel_ReturnCode200()
        {
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Parcel>(), It.IsAny<string>()))
                .Returns("PYJRB4HZ6");

            LogisticsPartnerApiController logisticsPartner = new(mockLogic.Object, mockMapper.Object);

            var validParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();
            var validTrackingId = "PYJRB4HZ6";

            var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void TransitionParcel_InvalidParcelZeroWeight_ReturnCode400()
        {
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Parcel>(), It.IsAny<string>()))
                .Returns(value: null);

            LogisticsPartnerApiController logisticsPartner = new(mockLogic.Object, mockMapper.Object);

            var invalidParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 0)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();
            var validTrackingId = "PYJRB4HZ6";

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }

        /*[Test]
        public void TransitionParcel_InvalidParcelRecipientIsNull_ReturnCode400()
        {
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Parcel>(), It.IsAny<string>()))
                .Returns(value: null);

            LogisticsPartnerApiController logisticsPartner = new(mockLogic.Object, mockMapper.Object);

            var invalidParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = null)
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();
            var trackingId = "PYJRB4HZ6";

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, trackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }

        [Test]
        public void TransitionParcel_InvalidParcelSenderIsNull_ReturnCode400()
        {
            mockLogic.Setup(m => m.TransitionParcel(It.IsAny<BusinessLogic.Entities.Parcel>(), It.IsAny<string>()))
                .Returns(value: null);

            LogisticsPartnerApiController logisticsPartner = new(mockLogic.Object, mockMapper.Object);

            var invalidParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = null)
                .Build();
            var trackingId = "PYJRB4HZ6";

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, trackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }*/

    }
}
