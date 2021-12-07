using AutoMapper;
using FizzWare.NBuilder;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.Services.Tests
{
    public class ParcelWebhookApiTest
    {
        Mock<IParcelWebhookLogic> mockLogic;
        Mock<IMapper> mockMapper;
        Mock<ILogger<ParcelWebhookApiController>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockLogic = new Mock<IParcelWebhookLogic>();
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<ParcelWebhookApiController>>();
        }

        [Test]
        public void ListParcelWebhooks_NoException_ReturnCode200()
        {
            var returnWebhooks = Builder<BusinessLogic.Entities.WebhookResponses>.CreateNew().Build();

            mockLogic.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Returns(returnWebhooks);

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);
            var validTrackingId = "PYJRB4HZ6";

            var testResult = webhookApi.ListParcelWebhooks(validTrackingId);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void ListParcelWebhooks_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                 .Throws(new LogicDataNotFoundException(null, null, null));

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);
            var invalidTrackingId = "PYJRB4HZ6";

            var testResult = webhookApi.ListParcelWebhooks(invalidTrackingId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }





        [Test]
        public void SubscribeParcelWebhook_NoException_ReturnCode200()
        {
            var returnWebhook = Builder<BusinessLogic.Entities.WebhookResponse>.CreateNew().Build();

            mockLogic.Setup(m => m.SubscribeParcelWebhook(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnWebhook);

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validTrackingId = "PYJRB4HZ6";
            var validUrl = "aURL";

            var testResult = webhookApi.SubscribeParcelWebhook(validTrackingId, validUrl);
            Assert.IsInstanceOf<ObjectResult>(testResult);
            var testResultCode = testResult as ObjectResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void SubscribeParcelWebhook_LogicDataNotFoundException_ReturnCode404()
        {
            mockLogic.Setup(m => m.SubscribeParcelWebhook(It.IsAny<string>(), It.IsAny<string>()))
                 .Throws(new LogicDataNotFoundException(null, null, null));

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validUrl = "aURL";
            var invalidTrackingId = "PYJRB4HZ6";

            var testResult = webhookApi.SubscribeParcelWebhook(invalidTrackingId, validUrl);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }






        [Test]
        public void UnsubscribeParcelWebhook_NoException_ReturnCode200()
        {

            Moq.Language.Flow.ISetup<IParcelWebhookLogic> setup = mockLogic.Setup(m => m.UnsubscribeParcelWebhook(It.IsAny<long>()));

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validId = 1;

            var testResult = webhookApi.UnsubscribeParcelWebhook(validId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(200, testResultCode.StatusCode);
        }

        [Test]
        public void UnsubscribeParcelWebhook_LogicDataNotFoundException_ReturnCode404()
        {

            mockLogic.Setup(m => m.UnsubscribeParcelWebhook(It.IsAny<long>()))
                 .Throws(new LogicDataNotFoundException(null, null, null));

            ParcelWebhookApiController webhookApi = new(mockLogic.Object, mockMapper.Object, mockLogger.Object);

            var validId = 1;

            var testResult = webhookApi.UnsubscribeParcelWebhook(validId);
            Assert.IsInstanceOf<StatusCodeResult>(testResult);
            var testResultCode = testResult as StatusCodeResult;

            Assert.AreEqual(404, testResultCode.StatusCode);
        }
    }
}
