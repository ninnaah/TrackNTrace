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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class RecipientLogic : IRecipientLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _repository;
        private readonly ILogger<RecipientLogic> _logger;
        public RecipientLogic(IParcelRepository repo, IMapper mapper, ILogger<RecipientLogic> logger)
        {
            _repository = repo;
            _mapper = mapper;
            _logger = logger;
        }

        public Parcel TrackParcel(string trackingId)
        {
            _logger.LogInformation("Trying to track a parcel");

            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                _logger.LogError($"Tracking id is invalid - {result}");
                throw new ValidatorException(nameof(trackingId), nameof(TrackParcel), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            try
            {
                DataAccess.Entities.Parcel DAparcel = _repository.GetSingleParcelByTrackingId(trackingId);
                Parcel parcel = this._mapper.Map<BusinessLogic.Entities.Parcel>(DAparcel);
                _logger.LogError("Tracked the parcel");
                return parcel;
            }
            catch(DataNotFoundException ex)
            {
                string errorMessage = $"Parcel with trackingid {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(RecipientLogic), nameof(TrackParcel), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred tracking a parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(RecipientLogic), nameof(TrackParcel), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred tracking a parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(RecipientLogic), nameof(TrackParcel), errorMessage, ex);
            }


        }
    }
}
