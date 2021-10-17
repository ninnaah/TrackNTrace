using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Mvc;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class RecipientLogicTest
    {
        [Test]
        public void TrackParcel_ValidTrackingId_ReturnParcel()
        {
            IRecipientLogic recipient= new RecipientLogic();

            string validTrackingId = "PYJRB4HZ6";

            var testResult = recipient.TrackParcel(validTrackingId);

            Assert.IsNotNull(testResult);
            Assert.IsInstanceOf<Parcel>(testResult);
        }

        [Test]
        public void TrackParcel_InvalidTrackingId_ReturnNull()
        {
            IRecipientLogic recipient = new RecipientLogic();

            string validTrackingId = "12";

            var testResult = recipient.TrackParcel(validTrackingId);

            Assert.IsNull(testResult);
        }

    }
}
