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
    public class StaffLogicTest
    {
        
        [Test]
        public void ReportParcelDelivery_ValidTrackingId_ReturnTrue()
        {
            IStaffLogic staff = new StaffLogic();

            string validTrackingId = "PYJRB4HZ6";

            var testResult = staff.ReportParcelDelivery(validTrackingId);

            Assert.IsTrue(testResult);
        }


        [Test]
        public void ReportParcelDelivery_InvalidTrackingId_ReturnFalse()
        {
            IStaffLogic staff = new StaffLogic();

            string invalidTrackingId = "12";

            var testResult = staff.ReportParcelDelivery(invalidTrackingId);

            Assert.IsFalse(testResult);
        }

        [Test]
        public void ReportParcelHop_ValidTrackingId_ReturnTrue()
        {
            IStaffLogic staff = new StaffLogic();

            string validTrackingId = "PYJRB4HZ6";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(validTrackingId, validCode);

            Assert.IsTrue(testResult);
        }


        [Test]
        public void ReportParcelHop_InvalidTrackingId_ReturnFalse()
        {
            IStaffLogic staff = new StaffLogic();

            string invalidTrackingId = "12";
            string validCode = "ABCD1234";

            var testResult = staff.ReportParcelHop(invalidTrackingId, validCode);

            Assert.IsFalse(testResult);
        }


    }
}
