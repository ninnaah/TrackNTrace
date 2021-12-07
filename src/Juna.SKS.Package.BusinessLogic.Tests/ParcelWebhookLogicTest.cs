using AutoMapper;
using FizzWare.NBuilder;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Tests
{
    public class ParcelWebhookLogicTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IWebhookRepository> mockWebhookRepo;
        Mock<IMapper> mockMapper;
        Mock<ILogger<ParcelWebhookLogic>> mockLogger;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockWebhookRepo = new Mock<IWebhookRepository>();

            mockMapper = new Mock<IMapper>();
           
            mockLogger = new Mock<ILogger<ParcelWebhookLogic>>();
        }

        [Test]
        public void ListParcelWebhooks_NoException_ReturnWebhooks()
        {
            mockMapper.Setup(m => m.Map<BusinessLogic.Entities.WebhookResponses>(It.IsAny<DataAccess.Entities.WebhookResponses>())).Returns(new BusinessLogic.Entities.WebhookResponses());
            var returnWebhooks = Builder<DataAccess.Entities.WebhookResponses>.CreateNew().Build();

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Returns(returnWebhooks);

            string validTrackingId = "PYJRB4HZ6";


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            var testResult = webhookLogic.ListParcelWebhooks(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponses>(testResult);
        }

        [Test]
        public void ListParcelWebhooks_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            string validTrackingId = "PYJRB4HZ6";

            try
            {
                webhookLogic.ListParcelWebhooks(validTrackingId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void ListParcelWebhooks_DataException_ThrowLogicException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            string validTrackingId = "PYJRB4HZ6";

            try
            {
                webhookLogic.ListParcelWebhooks(validTrackingId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }




        [Test]
        public void SubscribeParcelWebhook_NoException_ReturnWebhook()
        {
            mockMapper.Setup(m => m.Map<DataAccess.Entities.WebhookResponse>(It.IsAny<BusinessLogic.Entities.WebhookResponse>())).Returns(new DataAccess.Entities.WebhookResponse());

            mockWebhookRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.WebhookResponse>()))
                .Returns(1);

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";


            var testResult = webhookLogic.SubscribeParcelWebhook(validTrackingId, validUrl);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponse>(testResult);
        }

        [Test]
        public void SubscribeParcelWebhook_DataNotFoundExceptionGetParcel_ThrowLogicDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void SubscribeParcelWebhook_DataExceptionGetParcel_ThrowLogicException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }


        [Test]
        public void SubscribeParcelWebhook_DataExceptionCreate_ThrowLogicException()
        {
            mockMapper.Setup(m => m.Map<DataAccess.Entities.WebhookResponse>(It.IsAny<BusinessLogic.Entities.WebhookResponse>())).Returns(new DataAccess.Entities.WebhookResponse());

            mockWebhookRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.WebhookResponse>()))
                .Throws(new DataException(null, null));

            var returnParcel = Builder<DataAccess.Entities.Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }






        [Test]
        public void UnsubscribeParcelWebhook_NoException_DontThrowException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            long validId = 1;

            Assert.DoesNotThrow(() => webhookLogic.UnsubscribeParcelWebhook(validId));

        }

        [Test]
        public void UnsubscribeParcelWebhook_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            long invalidId = 1;

            try
            {
                webhookLogic.UnsubscribeParcelWebhook(invalidId);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UnsubscribeParcelWebhook_DataException_ThrowLogicException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new DataException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            long invalidId = 1;

            try
            {
                webhookLogic.UnsubscribeParcelWebhook(invalidId);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }
    }
}
