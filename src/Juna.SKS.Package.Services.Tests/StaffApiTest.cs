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
    public class StaffApiTest
    {
        Mock<IStaffLogic> mockLogic;
        Mock<IMapper> mockMapper;
        Mock<ILogger<StaffApiController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IStaffLogic>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<StaffApiController>>();
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_ReturnCode200()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void ReportParcelDelivery_ValidatorExceptionInvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()))
                .Throws(new ValidatorException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string invalidTrackingId = "123";

            var testResult = staff.ReportParcelDelivery(invalidTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }

        [Test]
        public void ReportParcelDelivery_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()))
                .Throws(new LogicDataNotFoundException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }

        [Test]
        public void ReportParcelDelivery_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()))
                .Throws(new LogicException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }





        [Test]
        public void ReportParcelHop_ValidTrackingIdValidCode_ReturnCode200()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void ReportParcelHop_ValidatorExceptionInvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ValidatorException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string invalidTrackingId = "123";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(invalidTrackingId, validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

        [Test]
        public void ReportParcelHop_ValidatorExceptionInvalidCode_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
              .Throws(new ValidatorException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string invalidCode = "123";

            var testResult = staff.ReportParcelHop(validTrackingId, invalidCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }

        [Test]
        public void ReportParcelHop_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
               .Throws(new LogicDataNotFoundException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }

        [Test]
        public void ReportParcelHop_LogicException_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
               .Throws(new LogicException(null, null, null));

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }


    }
}
