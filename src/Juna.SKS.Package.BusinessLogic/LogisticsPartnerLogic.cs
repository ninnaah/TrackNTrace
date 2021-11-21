using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

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
                _logger.LogError($"Parcel is invalid - {result}");
                throw new ValidatorException(nameof(parcel), nameof(TransitionParcel), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);

            try
            {
                _repository.Create(DAparcel);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred transitioning a parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred transitioning a parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage, ex);
            }

            _logger.LogInformation("Transitioned the parcel");
            return trackingId;
        }
    }
}
