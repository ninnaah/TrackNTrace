using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using FizzWare.NBuilder;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class LogisticsPartnerLogicTest
    {
        [Test]
        public void TransitionParcel_ValidParcel_ReturnTrackingId()
        {
            var validParcel = Builder<Parcel>.CreateNew()
                .With(p => p.Weight = 3)
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
                .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic();

            var testResult = logisticsPartner.TransitionParcel(validParcel, validTrackingId);

            Assert.AreEqual(validTrackingId, testResult);
        }

        [Test]
        public void TransitionParcel_InvalidParcelZeroWeight_ReturnNull()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
                .With(p => p.Weight = 0)
                .With(p => p.TrackingId = "PYJRB4HZ6")
                .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
                .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
                .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
                .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic();

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);

            Assert.IsNull(testResult);
        }

        [Test]
        public void TransitionParcel_InvalidParcelRecipientIsNull_ReturnNull()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = null)
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic();

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);

            Assert.IsNull(testResult);
        }

        [Test]
        public void TransitionParcel_InvalidParcelSenderIsNull_ReturnNull()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "PYJRB4HZ6")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = null)
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string validTrackingId = "PYJRB4HZ6";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic();

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, validTrackingId);

            Assert.IsNull(testResult);
        }

        [Test]
        public void TransitionParcel_InvalidParcelInvalidTrackingId_ReturnNull()
        {
            var invalidParcel = Builder<Parcel>.CreateNew()
               .With(p => p.Weight = 3)
               .With(p => p.TrackingId = "12")
               .With(p => p.Recipient = Builder<Recipient>.CreateNew().Build())
               .With(p => p.Sender = Builder<Recipient>.CreateNew().Build())
               .With(p => p.FutureHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .With(p => p.VisitedHops = Builder<HopArrival>.CreateListOfSize(3).Build().ToList())
               .Build();
            string invalidTrackingId = "12";

            ILogisticsPartnerLogic logisticsPartner = new LogisticsPartnerLogic();

            var testResult = logisticsPartner.TransitionParcel(invalidParcel, invalidTrackingId);

            Assert.IsNull(testResult);
        }

    }
}
