using AutoMapper;
using FizzWare.NBuilder;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.WebhookManager.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;

namespace Juna.SKS.Package.WebhookManager.Tests
{
    public class ParcelWebhookManagerTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IWebhookRepository> mockWebhookRepo;
        Mock<ILogger<ParcelWebhookManager>> mockLogger;
        Mock<IParcelWebhookManager> mockWebhookManager;
        Mock<HttpClient> mockHttpClient;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockWebhookRepo = new Mock<IWebhookRepository>();

            mockLogger = new Mock<ILogger<ParcelWebhookManager>>();

            mockWebhookManager = new Mock<IParcelWebhookManager>();

            mockHttpClient = new Mock<HttpClient>();
        }

        [Test]
        public void ListParcelWebhooks_NoException_ReturnWebhooks()
        {
            var returnWebhooks = Builder<WebhookResponses>.CreateNew().Build();

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Returns(returnWebhooks);

            string validTrackingId = "PYJRB4HZ6";


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            var testResult = webhookLogic.ListParcelWebhooks(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponses>(testResult);
        }

        [Test]
        public void ListParcelWebhooks_DataNotFoundException_ThrowDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            string validTrackingId = "PYJRB4HZ6";

            try
            {
                webhookLogic.ListParcelWebhooks(validTrackingId);
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void ListParcelWebhooks_DataException_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            string validTrackingId = "PYJRB4HZ6";

            try
            {
                webhookLogic.ListParcelWebhooks(validTrackingId);
                Assert.Fail();
            }
            catch (DataException)
            {
                Assert.Pass();
            }
        }




        [Test]
        public void SubscribeParcelWebhook_NoException_ReturnWebhook()
        {

            mockWebhookRepo.Setup(m => m.Create(It.IsAny<WebhookResponse>()))
                .Returns(1);

            var returnParcel = Builder<Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";


            var testResult = webhookLogic.SubscribeParcelWebhook(validTrackingId, validUrl);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponse>(testResult);
        }

        [Test]
        public void SubscribeParcelWebhook_DataNotFoundExceptionGetParcel_ThrowDataNotFoundException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void SubscribeParcelWebhook_DataExceptionGetParcel_ThrowDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (DataException)
            {
                Assert.Pass();
            }
        }


        [Test]
        public void SubscribeParcelWebhook_DataExceptionCreate_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.Create(It.IsAny<WebhookResponse>()))
                .Throws(new DataException(null, null));

            var returnParcel = Builder<Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            string invalidTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";

            try
            {
                webhookLogic.SubscribeParcelWebhook(invalidTrackingId, validUrl);
                Assert.Fail();
            }
            catch (DataException)
            {
                Assert.Pass();
            }
        }






        [Test]
        public void UnsubscribeParcelWebhook_NoException_DontThrowException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            long validId = 1;

            Assert.DoesNotThrow(() => webhookLogic.UnsubscribeParcelWebhook(validId));

        }

        [Test]
        public void UnsubscribeParcelWebhook_DataNotFoundException_ThrowDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            long invalidId = 1;

            try
            {
                webhookLogic.UnsubscribeParcelWebhook(invalidId);
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void UnsubscribeParcelWebhook_DataException_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            long invalidId = 1;

            try
            {
                webhookLogic.UnsubscribeParcelWebhook(invalidId);
                Assert.Fail();
            }
            catch (DataException)
            {
                Assert.Pass();
            }
        }



        /*[Test]
        public void NotifySubscribers_NoException_DontThrowException()
        {
            var webhooks = Builder<DataAccess.Entities.WebhookResponses>.CreateNew().Build();
            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>())).Returns(webhooks);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);

            var parcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            Assert.DoesNotThrow(() => webhookLogic.NotifySubscribers(parcel));

        }

        [Test]
        public void NotifySubscribers_DataNotFoundException_ThrowDataNotFoundException()
        {
            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            var parcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            try
            {
                webhookLogic.NotifySubscribers(parcel);
                Assert.Fail();
            }
            catch (DataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void NotifySubscribers_DataException_ThrowDataException()
        {
            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Throws(new DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object);
            var parcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            try
            {
                webhookLogic.NotifySubscribers(parcel);
                Assert.Fail();
            }
            catch (DataException)
            {
                Assert.Pass();
            }
        }*/
    }
}
