using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;


namespace Juna.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        public LogisticsPartnerLogic()
        {

        }
        public string TransitionParcel(Parcel parcel, string trackingId)
        {
            parcel.TrackingId = trackingId;
            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == true)
            {
                return trackingId;
            }

            return null;
        }
    }
}
