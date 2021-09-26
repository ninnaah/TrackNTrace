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
    public class SenderApiTest
    {

        [Test]
        public void SubmitParcel_ValidParcel_NoExceptionThrown()
        {
            Parcel parcel = new Parcel();
            SenderApiController sender = new SenderApiController();

            parcel.Weight = 25;
            Exception ex = null;

            try
            {
                sender.SubmitParcel(parcel);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.IsNull(ex);
        }

        [Test]
        public void SubmitParcel_ZeroWeight_ArgumentExceptionThrown()
        {
            Parcel parcel = new Parcel();
            SenderApiController sender = new SenderApiController();

            parcel.Weight = 0;
            Exception ex = null;

            try
            {
                sender.SubmitParcel(parcel);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Zero or negative weight is not valid");

        }

        [Test]
        public void SubmitParcel_EmptyWeight_ArgumentExceptionThrown()
        {
            Parcel parcel = new Parcel();
            SenderApiController sender = new SenderApiController();

            parcel.Weight = null;
            Exception ex = null;

            try
            {
                sender.SubmitParcel(parcel);
            }
            catch (Exception e)
            {
                ex = e;
            }

            Assert.AreEqual(ex.Message, "Empty weight is not valid");

        }

    }
}
