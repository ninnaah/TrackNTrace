using AutoMapper;
using FizzWare.NBuilder;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.WebhookManager.Interfaces;
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
        Mock<IMapper> mockMapper;
        Mock<ILogger<ParcelWebhookLogic>> mockLogger;
        Mock<IParcelWebhookManager> mockWebhookManager;

        [SetUp]
        public void Setup()
        {
            mockMapper = new Mock<IMapper>();
           
            mockLogger = new Mock<ILogger<ParcelWebhookLogic>>();

            mockWebhookManager = new Mock<IParcelWebhookManager>();
        }

        [Test]
        public void ListParcelWebhooks_NoException_ReturnWebhooks()
        {
            mockMapper.Setup(m => m.Map<WebhookResponses>(It.IsAny<DataAccess.Entities.WebhookResponses>())).Returns(new WebhookResponses());
            var returnWebhooks = Builder<DataAccess.Entities.WebhookResponses>.CreateNew().Build();

            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Returns(returnWebhooks);

            string validTrackingId = "PYJRB4HZ6";


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

            var testResult = webhookLogic.ListParcelWebhooks(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponses>(testResult);
        }

        [Test]
        public void ListParcelWebhooks_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
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
            mockWebhookManager.Setup(m => m.ListParcelWebhooks(It.IsAny<string>()))
                .Throws(new DataException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
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
            mockMapper.Setup(m => m.Map<WebhookResponse>(It.IsAny<DataAccess.Entities.WebhookResponse>())).Returns(new WebhookResponse());

            var returnWebhook = Builder<DataAccess.Entities.WebhookResponse>.CreateNew().Build();

            mockWebhookManager.Setup(m => m.SubscribeParcelWebhook(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(returnWebhook);


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

            string validTrackingId = "PYJRB4HZ6";
            string validUrl = "aURL";


            var testResult = webhookLogic.SubscribeParcelWebhook(validTrackingId, validUrl);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<WebhookResponse>(testResult);
        }

        [Test]
        public void SubscribeParcelWebhook_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookManager.Setup(m => m.SubscribeParcelWebhook(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new DataNotFoundException(null, null));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

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
        public void SubscribeParcelWebhook_DataException_ThrowLogicException()
        {
            mockWebhookManager.Setup(m => m.SubscribeParcelWebhook(It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new DataException(null, null));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

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
            mockWebhookManager.Setup(m => m.UnsubscribeParcelWebhook(It.IsAny<long>()));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

            long validId = 1;

            Assert.DoesNotThrow(() => webhookLogic.UnsubscribeParcelWebhook(validId));

        }

        [Test]
        public void UnsubscribeParcelWebhook_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookManager.Setup(m => m.UnsubscribeParcelWebhook(It.IsAny<long>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
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
            mockWebhookManager.Setup(m => m.UnsubscribeParcelWebhook(It.IsAny<long>()))
                .Throws(new DataException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
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





        [Test]
        public void NotifySubscribers_NoException_DontThrowException()
        {
            mockWebhookManager.Setup(m => m.NotifySubscribers(It.IsAny<DataAccess.Entities.Parcel>()));

            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);

            var parcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            Assert.DoesNotThrow(() => webhookLogic.NotifySubscribers(parcel));

        }

        [Test]
        public void NotifySubscribers_DataNotFoundException_ThrowLogicDataNotFoundException()
        {
            mockWebhookManager.Setup(m => m.NotifySubscribers(It.IsAny<DataAccess.Entities.Parcel>()))
                .Throws(new DataNotFoundException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
            var parcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            try
            {
                webhookLogic.NotifySubscribers(parcel);
                Assert.Fail();
            }
            catch (LogicDataNotFoundException)
            {
                Assert.Pass();
            }
        }

        [Test]
        public void NotifySubscribers_DataException_ThrowLogicException()
        {
            mockWebhookManager.Setup(m => m.NotifySubscribers(It.IsAny<DataAccess.Entities.Parcel>()))
                .Throws(new DataException(null, null));


            IParcelWebhookLogic webhookLogic = new ParcelWebhookLogic(mockMapper.Object, mockLogger.Object, mockWebhookManager.Object);
            var parcel = Builder<BusinessLogic.Entities.Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.Recipient = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<BusinessLogic.Entities.Recipient>.CreateNew().Build())
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.VisitedHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.FutureHops = Builder<BusinessLogic.Entities.HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();

            try
            {
                webhookLogic.NotifySubscribers(parcel);
                Assert.Fail();
            }
            catch (LogicException)
            {
                Assert.Pass();
            }
        }
    }
}
