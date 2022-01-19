using AutoMapper;
using FizzWare.NBuilder;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.WebhookManager.Interfaces;
using Juna.SKS.Package.WebhookManager.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Juna.SKS.Package.WebhookManager.Tests
{
    public class ParcelWebhookManagerTest
    {
        Mock<IParcelRepository> mockParcelRepo;
        Mock<IWebhookRepository> mockWebhookRepo;
        Mock<ILogger<ParcelWebhookManager>> mockLogger;
        Mock<HttpMessageHandler> mockHttpHandler;

        [SetUp]
        public void Setup()
        {
            mockParcelRepo = new Mock<IParcelRepository>();
            mockWebhookRepo = new Mock<IWebhookRepository>();

            mockLogger = new Mock<ILogger<ParcelWebhookManager>>();


            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            };

            mockHttpHandler = new Mock<HttpMessageHandler>();
            mockHttpHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);
        }

        [Test]
        public void ListParcelWebhooks_NoException_ReturnWebhooks()
        {
            var returnWebhooks = Builder<WebhookResponses>.CreateNew().Build();

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Returns(returnWebhooks);

            string validTrackingId = "PYJRB4HZ6";


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

            var testResult = webhookLogic.ListParcelWebhooks(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponses>(testResult);
        }

        [Test]
        public void ListParcelWebhooks_DataNotFoundException_ThrowDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataAccess.Interfaces.Exceptions.DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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
                .Throws(new DataAccess.Interfaces.Exceptions.DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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
        public void ListParcelWebhooks_Exception_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new Exception(null,null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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
                .Throws(new DataAccess.Interfaces.Exceptions.DataNotFoundException(null, null));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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
                .Throws(new DataAccess.Interfaces.Exceptions.DataException(null, null));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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
        public void SubscribeParcelWebhook_ExceptionGetParcel_ThrowDataException()
        {
            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Throws(new Exception(null, null));

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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
                .Throws(new DataAccess.Interfaces.Exceptions.DataException(null, null));

            var returnParcel = Builder<Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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
        public void SubscribeParcelWebhook_ExceptionCreate_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.Create(It.IsAny<WebhookResponse>()))
                .Throws(new Exception(null, null));

            var returnParcel = Builder<Parcel>.CreateNew().Build();

            mockParcelRepo.Setup(m => m.GetSingleParcelByTrackingId(It.IsAny<string>()))
                .Returns(returnParcel);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

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

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);

            long validId = 1;

            Assert.DoesNotThrow(() => webhookLogic.UnsubscribeParcelWebhook(validId));

        }

        [Test]
        public void UnsubscribeParcelWebhook_DataNotFoundException_ThrowDataNotFoundException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new DataAccess.Interfaces.Exceptions.DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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
                .Throws(new DataAccess.Interfaces.Exceptions.DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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

        [Test]
        public void UnsubscribeParcelWebhook_Exception_ThrowDataException()
        {
            mockWebhookRepo.Setup(m => m.Delete(It.IsAny<long>()))
                .Throws(new Exception(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, null);
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




        [Test]
        public void NotifySubscribers_NoException_DontThrowException()
        {
            var httpClient = new HttpClient(mockHttpHandler.Object);
            List<WebhookResponse> webhooks = new List<WebhookResponse>()
            {
                new WebhookResponse("ABC", "http://example.com", DateTime.Now),
                new WebhookResponse("ABC", "http://example.com", DateTime.Now),
                new WebhookResponse("ABC", "http://example.com", DateTime.Now)
            };

            DataAccess.Entities.WebhookResponses webhookResponses = new();

            foreach(WebhookResponse webhook in webhooks)
            {
                webhookResponses.Add(webhook);
            }

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
               .Returns(webhookResponses);

            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, httpClient);

            var parcel = Builder<DataAccess.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<DataAccess.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<DataAccess.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            Assert.DoesNotThrow(() => webhookLogic.NotifySubscribers(parcel));
            mockHttpHandler.Protected().Verify(
              "SendAsync",
              Times.Exactly(3),
              ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
              ItExpr.IsAny<CancellationToken>());

        }

        [Test]
        public void NotifySubscribers_DataNotFoundException_ThrowDataNotFoundException()
        {
            var httpClient = new HttpClient(mockHttpHandler.Object);

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataAccess.Interfaces.Exceptions.DataNotFoundException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, httpClient);
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
            var httpClient = new HttpClient(mockHttpHandler.Object);

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new DataAccess.Interfaces.Exceptions.DataException(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, httpClient);
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
        }

        [Test]
        public void NotifySubscribers_Exception_ThrowDataException()
        {
            var httpClient = new HttpClient(mockHttpHandler.Object);

            mockWebhookRepo.Setup(m => m.GetWebhookResponsesByTrackingId(It.IsAny<string>()))
                .Throws(new Exception(null, null));


            IParcelWebhookManager webhookLogic = new ParcelWebhookManager(mockWebhookRepo.Object, mockParcelRepo.Object, mockLogger.Object, httpClient);
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
        }
    }
}
