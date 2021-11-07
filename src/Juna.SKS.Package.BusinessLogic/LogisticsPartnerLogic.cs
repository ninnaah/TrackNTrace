using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;


namespace Juna.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        public LogisticsPartnerLogic(IParcelRepository repo, IMapper mapper)
        {
            _repository = repo;
            _mapper = mapper;
        }
        public string TransitionParcel(Parcel parcel, string trackingId)
        {
            parcel.TrackingId = trackingId;
            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == true)
            {
                DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _repository.Create(DAparcel);
                return trackingId;
            }

            return null;
        }
    }
}
