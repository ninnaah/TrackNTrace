using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;


namespace Juna.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        private readonly ILogger<LogisticsPartnerLogic> _logger;
        public LogisticsPartnerLogic(IParcelRepository repo, IMapper mapper, ILogger<LogisticsPartnerLogic> logger)
        {
            _repository = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public string TransitionParcel(Parcel parcel, string trackingId)
        {
            _logger.LogInformation("Trying to transition a parcel");

            parcel.TrackingId = trackingId;
            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == false)
            {
                _logger.LogDebug($"Parcel is invalid - {result}");
                return null;
            }

            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
            _repository.Create(DAparcel);
            _logger.LogInformation("Transitioned the parcel");
            return trackingId;
        }
    }
}
