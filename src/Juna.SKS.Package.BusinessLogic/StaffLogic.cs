using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        public StaffLogic()
        {

        }
        public bool ReportParcelDelivery(string trackingId)
        {
            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                return false;
            }
            return true;
        }

        public bool ReportParcelHop(string trackingId, string code)
        {
            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                return false;
            }

            return true;
        }
    }
}
