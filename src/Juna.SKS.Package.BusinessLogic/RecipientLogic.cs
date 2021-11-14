using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces;
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
                _logger.LogDebug($"Tracking id is invalid - {result}");
                return null;
            }

            try
            {
                DataAccess.Entities.Parcel DAparcel = _repository.GetSingleParcelByTrackingId(trackingId);
                Parcel parcel = this._mapper.Map<BusinessLogic.Entities.Parcel>(DAparcel);
                _logger.LogInformation("Tracked the parcel");
                return parcel;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Parcel not found - {ex.Message}");
                throw;
            }
            
            
        }
    }
}
