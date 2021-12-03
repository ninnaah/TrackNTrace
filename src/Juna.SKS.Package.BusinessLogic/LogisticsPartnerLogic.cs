using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Juna.SKS.Package.BusinessLogic
{
    public class LogisticsPartnerLogic : ILogisticsPartnerLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepo;
        private readonly IHopRepository _hopRepo;
        private readonly ILogger<LogisticsPartnerLogic> _logger;
        private readonly IGeoEncodingAgent _agent;
        public LogisticsPartnerLogic(IParcelRepository parcelRepo, IHopRepository hopRepo, IMapper mapper, ILogger<LogisticsPartnerLogic> logger, IGeoEncodingAgent agent)
        {
            _parcelRepo = parcelRepo;
            _hopRepo = hopRepo;
            _mapper = mapper;
            _logger = logger;
            _agent = agent;
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

            _logger.LogInformation("Get coordinates of sender and recipient");

            GeoCoordinate senderCoordinates = new();
            GeoCoordinate recipientCoordinates = new();

            try
            {
                DataAccess.Entities.GeoCoordinate DAsenderCoordinates = _agent.EncodeAddress(parcel.Sender.Street, parcel.Sender.PostalCode, parcel.Sender.City, parcel.Sender.Country);
                senderCoordinates = this._mapper.Map<BusinessLogic.Entities.GeoCoordinate>(DAsenderCoordinates);

                DataAccess.Entities.GeoCoordinate DArecipientCoordinates = _agent.EncodeAddress(parcel.Recipient.Street, parcel.Recipient.PostalCode, parcel.Recipient.City, parcel.Recipient.Country);
                recipientCoordinates = this._mapper.Map<BusinessLogic.Entities.GeoCoordinate>(DArecipientCoordinates);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"GeoCoordinates cannot be encoded";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred encoding geoCoordinates";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage, ex);
            }

            _logger.LogInformation("Trying to predict future hops");
            _logger.LogInformation("Find out trucks of sender and recipient");

            IEnumerable<DataAccess.Entities.Hop> DAtrucks = new List<DataAccess.Entities.Hop>();
            try
            {
                DAtrucks = _hopRepo.GetHopsByHopType("Truck");
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Trucks cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred getting trucks";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An error occurred getting trucks";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(LogisticsPartnerLogic), nameof(TransitionParcel), errorMessage, ex);
            }


            DataAccess.Entities.Truck senderTruck = new();
            DataAccess.Entities.Truck recipientTruck = new();

            foreach (DataAccess.Entities.Truck truck in DAtrucks)
            {
                if (truck.Region.Contains(Geometry.DefaultFactory.CreatePoint(new Coordinate(senderCoordinates.Lon, senderCoordinates.Lat))))
                    senderTruck = truck;

                if (truck.Region.Contains(Geometry.DefaultFactory.CreatePoint(new Coordinate(recipientCoordinates.Lon, recipientCoordinates.Lat))))
                    recipientTruck = truck;
            }
            if (senderTruck == recipientTruck)
            {
                _logger.LogInformation("Sender and recipient are in the same region");
                parcel.FutureHops.Add(new HopArrival(recipientTruck.Code, recipientTruck.Description, DateTime.Now));

                _logger.LogInformation("Set parcel state to inTruckDelivery");
                parcel.State = Parcel.StateEnum.InTruckDeliveryEnum;
            }

            else
            {
                _logger.LogInformation("Predicting future hops - stepping through hierarchy");

                parcel.FutureHops = new List<HopArrival>
                {
                    new HopArrival(senderTruck.Parent.Hop.Code, senderTruck.Parent.Hop.Description, DateTime.Now),
                    new HopArrival(recipientTruck.Parent.Hop.Code, recipientTruck.Parent.Hop.Description, DateTime.Now)
                };

                DataAccess.Entities.Warehouse currentSenderHop = senderTruck.Parent.Parent;
                DataAccess.Entities.Warehouse currentRecipientHop = recipientTruck.Parent.Parent;

                while (currentSenderHop != currentRecipientHop)
                {
                    currentSenderHop = currentSenderHop.Parent.Parent;
                    currentRecipientHop = currentRecipientHop.Parent.Parent;
                    parcel.FutureHops.Add(new HopArrival(currentSenderHop.Code, currentSenderHop.Description, DateTime.Now));
                    parcel.FutureHops.Add(new HopArrival(currentRecipientHop.Code, currentRecipientHop.Description, DateTime.Now));
                }

                parcel.FutureHops.Add(new HopArrival(currentSenderHop.Code, currentSenderHop.Description, DateTime.Now));
                _logger.LogInformation("Predicted future hops");

                _logger.LogInformation("Set parcel state to pickup");
                parcel.State = Parcel.StateEnum.PickupEnum;

            }

            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);

            try
            {
                _parcelRepo.Create(DAparcel);
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
