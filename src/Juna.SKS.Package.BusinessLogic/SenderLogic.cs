using AutoMapper;
using Fare;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.ServiceAgents;
using Juna.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Juna.SKS.Package.BusinessLogic
{
    public class SenderLogic : ISenderLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepo;
        private readonly IHopRepository _hopRepo;
        private readonly ILogger<SenderLogic> _logger;
        private readonly IGeoEncodingAgent _agent;
        public SenderLogic(IParcelRepository parcelRepo, IHopRepository hopRepo, IMapper mapper, ILogger<SenderLogic> logger, IGeoEncodingAgent agent)
        {
            _parcelRepo = parcelRepo;
            _hopRepo = hopRepo;
            _mapper = mapper;
            _logger = logger;
            _agent = agent;
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

            parcel.TrackingId = generateTrackingId();

            //get GPS coordinates
            DataAccess.Entities.GeoCoordinate DAsenderCoordinates = _agent.EncodeAddress(parcel.Sender.Street, parcel.Sender.PostalCode, parcel.Sender.City, parcel.Sender.Country);
            GeoCoordinate senderCoordinates = this._mapper.Map<BusinessLogic.Entities.GeoCoordinate>(DAsenderCoordinates);

            DataAccess.Entities.GeoCoordinate DArecipientCoordinates = _agent.EncodeAddress(parcel.Recipient.Street, parcel.Recipient.PostalCode, parcel.Recipient.City, parcel.Recipient.Country);
            GeoCoordinate recipientCoordinates = this._mapper.Map<BusinessLogic.Entities.GeoCoordinate>(DArecipientCoordinates);

            //predict future hops
            /*IEnumerable<DataAccess.Entities.Truck> DAtrucks = _hopRepo.GetTrucks();
            DataAccess.Entities.Truck senderTruck = new();
            DataAccess.Entities.Truck recipientTruck = new();
            foreach (DataAccess.Entities.Truck truck in DAtrucks)
            {
                if (truck.Region.Contains(new Point(senderCoordinates.Lat, senderCoordinates.Lon)))
                    senderTruck = truck;

                if (truck.Region.Contains(new Point(recipientCoordinates.Lat, senderCoordinates.Lon)))
                    recipientTruck = truck;
            }
            if (senderTruck == recipientTruck)
                parcel.FutureHops.Add(new HopArrival(senderTruck.Code, senderTruck.Description, DateTime.Now));
            else
            {

                //do smthg


            }*/

            parcel.State = Parcel.StateEnum.PickupEnum;

            DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
            try
            {
                _parcelRepo.Create(DAparcel);
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

        public string generateTrackingId()
        {
            var xeger = new Xeger("^[A-Z0-9]{9}$", new Random());
            string trackingId = xeger.Generate();
            _logger.LogInformation($"Generated new trackingId {trackingId}");

            //check if id is unique
            /*try
            {
                _parcelRepo.GetSingleParcelByTrackingId(trackingId);
            }
            catch (DataNotFoundException)
            {
                return trackingId;
                _logger.LogError($"Tracking id {trackingId} already exist - retry");
            }*/

            return trackingId;
        }
    }
}
