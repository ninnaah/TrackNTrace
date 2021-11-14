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
    public class StaffLogic : IStaffLogic
    {
        private readonly IParcelRepository _parcelRepo;
        private readonly IHopRepository _hopRepo;
        private readonly ILogger<StaffLogic> _logger;
        public StaffLogic(IParcelRepository parcelRepo, IHopRepository hopRepo, ILogger<StaffLogic> logger)
        {
            _parcelRepo = parcelRepo;
            _hopRepo = hopRepo;
            _logger = logger;
        }
        public bool ReportParcelDelivery(string trackingId)
        {
            _logger.LogInformation("Trying to report a parcel delivery");
            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                _logger.LogDebug($"Tracking id is invalid - {result}");
                return false;
            }

            try
            {
                DataAccess.Entities.Parcel parcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
                if (parcel.State == DataAccess.Entities.Parcel.StateEnum.DeliveredEnum)
                {
                    _logger.LogInformation("Reported the parcel delivery");
                    return true;
                }
                else
                {
                    _logger.LogDebug("Cannot report the parcel delivery - not delivered yet");
                    return false;
                }
            }catch (Exception ex)
            {
                _logger.LogError($"Parcel not found - {ex.Message}");
                throw;
            }
            
                
        }

        public bool ReportParcelHop(string trackingId, string code)
        {
            _logger.LogInformation("Trying to report a parcel hop");

            Parcel wrapParcel = new Parcel(3, new Recipient(), new Recipient(), trackingId, new List<HopArrival>(), new List<HopArrival>(), Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> parcelValidator = new ParcelValidator();
            var result = parcelValidator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                _logger.LogError($"Tracking id is invalid - {result}");
                return false;
            }

            HopArrival wrapHopArrival = new HopArrival(code, "Hauptlager 27-12", DateTime.Now);

            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            result = hopArrivalValidator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                _logger.LogError($"Code is invalid - {result}");
                return false;
            }

            try
            {
                DataAccess.Entities.Parcel parcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
                DataAccess.Entities.HopArrival hop = _hopRepo.GetSingleHopArrivalByCode(code);

                foreach (DataAccess.Entities.HopArrival h in parcel.VisitedHops)
                {
                    if (h.Code == hop.Code)
                    {
                        _logger.LogInformation("Reported the parcel hop");
                        return true;
                    }

                }

                _logger.LogError("Cannot report the parcel hop - parcel not delivered at hop yet");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Parcel or hop not found - {ex.Message}");
                throw;
            }
            
        }
    }
}
