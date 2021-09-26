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
    public class LogisticsPartnerApiTest
    {
        [Test]
        public void TransitionParcel_ValidParcel_NoExceptionThrown()
        {
            Parcel parcel = new Parcel();
            LogisticsPartnerApiController logisticsPartner = new LogisticsPartnerApiController();

            string trackingId = "H7Z4R2DF5";
            parcel.Weight = 25;
            Exception ex = null;

            try
            {
                logisticsPartner.TransitionParcel(parcel, trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNull(ex);
        }

        [Test]
        public void TransitionParcel_ZeroWeight_ArgumentExceptionThrown()
        {
            Parcel parcel = new Parcel();
            LogisticsPartnerApiController logisticsPartner = new LogisticsPartnerApiController();

            string trackingId = "H7Z4R2DF5";
            parcel.Weight = 0;
            Exception ex = null;

            try
            {
                logisticsPartner.TransitionParcel(parcel, trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Zero or negative weight is not valid");

        }

        [Test]
        public void TransitionParcel_EmptyWeight_ArgumentExceptionThrown()
        {
            Parcel parcel = new Parcel();
            LogisticsPartnerApiController logisticsPartner = new LogisticsPartnerApiController();

            string trackingId = "H7Z4R2DF5";
            parcel.Weight = null;
            Exception ex = null;

            try
            {
                logisticsPartner.TransitionParcel(parcel, trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Empty weight is not valid");

        }
    }
}
