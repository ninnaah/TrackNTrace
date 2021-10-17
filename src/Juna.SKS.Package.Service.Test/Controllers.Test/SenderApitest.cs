﻿using System;
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

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class SenderApiTest
    {
        Mock<ISenderLogic> mockLogic;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<ISenderLogic>();
            mockMapper = new Mock<IMapper>();
        }

        [Test]
        public void SubmitParcel_ValidParcel_ReturnCode201()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
                .Returns("PYJRB4HZ6");

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object);

            var validParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(validParcel);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(201, testResultCode.StatusCode);
        }

        [Test]
        public void SubmitParcel_InvalidParcelZeroWeight_ReturnCode400()
        {
            mockLogic.Setup(m => m.SubmitParcel(It.IsAny<BusinessLogic.Entities.Parcel>()))
               .Returns(value: null);

            SenderApiController sender = new(mockLogic.Object, mockMapper.Object);

            var invalidParcel = Builder<DTOs.Models.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DTOs.Models.Recipient>.CreateNew().Build())
                .Build();

            var testResult = sender.SubmitParcel(invalidParcel);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

    }
}
