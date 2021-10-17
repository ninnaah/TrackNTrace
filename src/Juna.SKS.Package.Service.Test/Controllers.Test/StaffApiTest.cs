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
    public class StaffApiTest
    {
        Mock<IStaffLogic> mockLogic;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IStaffLogic>();
            mockMapper = new Mock<IMapper>();
        }

        [Test]
        public void ReportParcelDelivery_ValidTrackingId_ReturnCode200()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()))
                .Returns(true);

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object);

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void ReportParcelDelivery_InvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelDelivery(It.IsAny<string>()))
                .Returns(false);

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object);

            string invalidTrackingId = "123";

            var testResult = staff.ReportParcelDelivery(invalidTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);
        }

        [Test]
        public void ReportParcelHop_ValidTrackingId_ReturnCode200()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
               .Returns(true);

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }


        [Test]
        public void ReportParcelHop_InvalidTrackingId_ReturnCode400()
        {
            mockLogic.Setup(m => m.ReportParcelHop(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(false);

            StaffApiController staff = new(mockLogic.Object, mockMapper.Object);

            string invalidTrackingId = "123";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(invalidTrackingId, validCode);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(400, testResultCode.StatusCode);

        }


    }
}
