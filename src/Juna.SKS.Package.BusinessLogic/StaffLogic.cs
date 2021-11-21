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
                _logger.LogError($"Tracking id is invalid - {result}");
                throw new ValidatorException(nameof(trackingId), nameof(ReportParcelDelivery), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
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
                    string errorMessage = $"Cannot report the parcel delivery with trackingid {trackingId} - not delivered yet";
                    _logger.LogError(errorMessage);
                    throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage);
                }
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Parcel with trackingid {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred reporting a parcel delivery with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred reporting a parcel delivery with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
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
                throw new ValidatorException(nameof(trackingId), nameof(ReportParcelHop), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            HopArrival wrapHopArrival = new HopArrival(code, "Hauptlager 27-12", DateTime.Now);

            IValidator<HopArrival> hopArrivalValidator = new HopArrivalValidator();
            result = hopArrivalValidator.Validate(wrapHopArrival);

            if (result.IsValid == false)
            {
                _logger.LogError($"Code is invalid - {result}");
                throw new ValidatorException(nameof(code), nameof(ReportParcelHop), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
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

                string errorMessage = $"Cannot report the parcel hop with trackingid {trackingId} and code {code} - not delivered yet";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = null;
                if (ex.Method == "GetSingleParcelByTrackingId")
                {
                    errorMessage = $"Parcel with trackingid {trackingId} cannot be found";
                }else if(ex.Method == "GetSingleHopArrivalByCode")
                {
                    errorMessage = $"Hop with code {code} cannot be found";
                }
                
                _logger.LogError(errorMessage);
                throw new LogicDataNotFoundException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred reporting a parcel hop with trackingid {trackingId} and code {code}";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred reporting a parcel hop with trackingid {trackingId} and code {code}";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
            }

        }
    }
}
