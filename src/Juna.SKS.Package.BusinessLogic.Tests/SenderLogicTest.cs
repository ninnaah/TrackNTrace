using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic;
using Moq;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.DataAccess.Interfaces;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class SenderLogicTest
    {
        Mock<IParcelRepository> mockRepo;
        Mock<IMapper> mockMapper;

        [SetUp]
        public void Setup()
        {
            mockRepo = new Mock<IParcelRepository>();
            mockRepo.Setup(m => m.Create(It.IsAny<DataAccess.Entities.Parcel>()))
                .Returns(1);

            mockMapper = new Mock<IMapper>();
        }
        [Test]
        public void SubmitParcel_ValidParcel_ReturnTrackingId()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.AreEqual(validParcel.TrackingId, testResult);
        }

        [Test]
        public void SubmitParcel_InvalidParcelInvalidTrackingId_ReturnNull()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "12")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.IsNull(testResult);
        }

        [Test]
        public void SubmitParcel_InvalidParcelZeroWeight_ReturnNull()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 0)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.IsNull(testResult);

        }

        [Test]
        public void SubmitParcel_InvalidParcelRecipientIsNull_ReturnNull()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = null)
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.IsNull(testResult);

        }

        [Test]
        public void SubmitParcel_InvalidParcelSenderIsNull_ReturnNull()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = null)
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.IsNull(testResult);

        }
        [Test]
        public void SubmitParcel_InvalidParcelFutureHopsIsNull_ReturnNull()
        {
            var validParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = null)
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();

            ISenderLogic sender = new SenderLogic(mockRepo.Object, mockMapper.Object);

            var testResult = sender.SubmitParcel(validParcel);

            Assert.IsNull(testResult);

        }

    }
}
