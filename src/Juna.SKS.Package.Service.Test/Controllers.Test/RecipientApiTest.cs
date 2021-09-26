using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Juna.SKS.Package.Services.DTOs.Models;
using Juna.SKS.Package.Services.Controllers;

namespace Juna.SKS.Package.Services.Test.Controllers.Test
{
    public class RecipientApiTest
    {
        [Test]
        public void TrackParcel_ValidTrackingId_NoArgumentExceptionThrown()
        {
            RecipientApiController recipient = new RecipientApiController();

            string trackingId = "H7Z4R2DF5";
            Exception ex = null;

            try
            {
                recipient.TrackParcel(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNull(ex);

        }

        [Test]
        public void TrackParcel_TrackingIdLengthZero_ArgumentExceptionThrown()
        {
            RecipientApiController recipient = new RecipientApiController();

            string trackingId = "";
            Exception ex = null;

            try
            {
                recipient.TrackParcel(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot have zero or negative length");

        }


        [Test]
        public void TrackParcel_EmptyTrackingId_ArgumentExceptionThrown()
        {
            RecipientApiController recipient = new RecipientApiController();

            string trackingId = null;
            Exception ex = null;

            try
            {
                recipient.TrackParcel(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot be null");

        }

    }
}
