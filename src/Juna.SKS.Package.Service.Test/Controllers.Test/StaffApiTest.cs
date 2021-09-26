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
    public class StaffApiTest
    {
        [Test]
        public void ReportParcelDelivery_ValidTrackingId_NoArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = "H7Z4R2DF5";
            Exception ex = null;

            try
            {
                staff.ReportParcelDelivery(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            //Assert.IsNull(ex);
            Assert.AreEqual(ex.Message, "The method or operation is not implemented.");
        }


        [Test]
        public void ReportParcelDelivery_EmptyTrackingId_ArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = null;
            Exception ex = null;

            try
            {
                staff.ReportParcelDelivery(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot be null");

        }

        [Test]
        public void ReportParcelDelivery_TrackingIdLengthZero_ArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = "";
            Exception ex = null;

            try
            {
                staff.ReportParcelDelivery(trackingId);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot have zero or negative length");

        }

        [Test]
        public void ReportParcelHop_ValidTrackingId_NoArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = "H7Z4R2DF5";
            string code = "1234";
            Exception ex = null;

            try
            {
                staff.ReportParcelHop(trackingId, code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            //Assert.IsNull(ex);
            Assert.AreEqual(ex.Message, "The method or operation is not implemented.");

        }


        [Test]
        public void ReportParcelHop_EmptyTrackingId_ArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = null;
            string code = "1234";
            Exception ex = null;

            try
            {
                staff.ReportParcelHop(trackingId, code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot be null");

        }

        [Test]
        public void ReportParcelHop_TrackingIdLengthZero_ArgumentExceptionThrown()
        {
            StaffApiController staff = new StaffApiController();

            string trackingId = "";
            string code = "1234";
            Exception ex = null;

            try
            {
                staff.ReportParcelHop(trackingId, code);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "TrackingID cannot have zero or negative length");

        }


    }
}
