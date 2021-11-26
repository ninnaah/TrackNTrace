using AutoMapper;
using Fare;
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
    public class SenderLogic : ISenderLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        private readonly ILogger<SenderLogic> _logger;
        public SenderLogic(IParcelRepository repo, IMapper mapper, ILogger<SenderLogic> logger)
        {
            _repository = repo;
            _mapper = mapper;
            _logger = logger;
        }
        public string SubmitParcel(Parcel parcel)
        {
            _logger.LogInformation("Trying to submit a parcel");

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(parcel);

            if (result.IsValid == false)
            {
                _logger.LogError($"Parcel is invalid - {result}");
                throw new ValidatorException(nameof(parcel), nameof(SubmitParcel), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            var xeger = new Xeger("^[A-Z0-9]{9}$", new Random());
            var trackingId = xeger.Generate();

            //check if id is unique
            parcel.TrackingId = trackingId;


            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
            try
            {
                _repository.Create(DAparcel);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred submitting a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(SenderLogic), nameof(SubmitParcel), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred submitting a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(SenderLogic), nameof(SubmitParcel), errorMessage, ex);
            }
            _logger.LogInformation("Submitted the parcel");
            return parcel.TrackingId;
        }
    }
}
