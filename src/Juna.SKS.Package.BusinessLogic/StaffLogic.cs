using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository _parcelRepo;
        private readonly IHopRepository _hopRepo;
        public StaffLogic(IParcelRepository parcelRepo, IHopRepository hopRepo)
        {
            _parcelRepo = parcelRepo;
            _hopRepo = hopRepo;
        }
        public bool ReportParcelDelivery(string trackingId)
        {
            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
                return false;

            DataAccess.Entities.Parcel parcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
            if (parcel.State == DataAccess.Entities.Parcel.StateEnum.DeliveredEnum)
                return true;
            else
                return false;
        }

        public bool ReportParcelHop(string trackingId, string code)
        {
            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
                return false;

            DataAccess.Entities.Parcel parcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
            DataAccess.Entities.HopArrival hop = _hopRepo.GetSingleHopArrivalByCode(code);

            foreach(DataAccess.Entities.HopArrival h in parcel.VisitedHops)
            {
                if(h.Code == hop.Code)
                    return true;
            }

            return false;
        }
    }
}
