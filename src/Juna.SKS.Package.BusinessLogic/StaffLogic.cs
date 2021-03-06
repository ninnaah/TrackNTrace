using AutoMapper;
using FluentValidation;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Entities.Validators;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class StaffLogic : IStaffLogic
    {
        private readonly IMapper _mapper;
        private readonly IParcelRepository _parcelRepo;
        private readonly IHopRepository _hopRepo;
        private readonly ILogger<StaffLogic> _logger;
        private readonly IParcelWebhookLogic _webhookLogic;
        public StaffLogic(IParcelRepository parcelRepo, IHopRepository hopRepo, IMapper mapper, ILogger<StaffLogic> logger, IParcelWebhookLogic webhookLogic)
        {
            _parcelRepo = parcelRepo;
            _hopRepo = hopRepo;
            _mapper = mapper;
            _logger = logger;
            _webhookLogic = webhookLogic;
        }
        public void ReportParcelDelivery(string trackingId)
        {
            _logger.LogInformation("Trying to report a parcel delivery");
            Parcel wrapParcel = new Parcel(3, new(), new(), trackingId, null, null, Parcel.StateEnum.InTransportEnum);

            IValidator<Parcel> validator = new ParcelValidator();
            var result = validator.Validate(wrapParcel);

            if (result.IsValid == false)
            {
                _logger.LogError($"Tracking id is invalid - {result}");
                throw new ValidatorException(nameof(trackingId), nameof(ReportParcelDelivery), string.Join(" ", result.Errors.Select(err => err.ErrorMessage)));
            }

            Parcel parcel = new();

            try
            {
                DataAccess.Entities.Parcel DAparcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
                parcel = this._mapper.Map<BusinessLogic.Entities.Parcel>(DAparcel);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Parcel with trackingid {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage);
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


            if (parcel.State == Parcel.StateEnum.DeliveredEnum)
            {
                _logger.LogInformation($"Parcel with trackingId {trackingId} is already delivered");
            }
            else
            {
                parcel.State = Parcel.StateEnum.DeliveredEnum;
                try
                {
                    foreach (HopArrival hop in parcel.FutureHops)
                    {
                        parcel.VisitedHops.Add(hop);
                    }
                    parcel.FutureHops.Clear();
                    var webhooks = _webhookLogic.ListParcelWebhooks(parcel.TrackingId);
                    foreach (WebhookResponse webhook in webhooks)
                    {
                        _webhookLogic.UnsubscribeParcelWebhook(webhook.Id);
                    }

                    DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
                    _parcelRepo.Update(DAparcel);
                }
                catch (LogicDataNotFoundException ex)
                {
                    string errorMessage = $"Webhooks with trackingId {trackingId} cannot be found";
                    _logger.LogError(errorMessage, ex);
                    throw new LogicDataNotFoundException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage);
                }
                catch (DataException ex)
                {
                    string errorMessage = $"An error occurred updating the parcel with trackingid {trackingId}";
                    _logger.LogError(errorMessage, ex);
                    throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
                }
                catch (Exception ex)
                {
                    string errorMessage = $"An unknown error occurred updating the parcel with trackingid {trackingId}";
                    _logger.LogError(errorMessage, ex);
                    throw new LogicException(nameof(StaffLogic), nameof(ReportParcelDelivery), errorMessage, ex);
                }

                _logger.LogInformation($"Parcel with trackingId {trackingId} is now delivered");
            }
            return;

        }

        public void ReportParcelHop(string trackingId, string code)
        {
            _logger.LogInformation("Trying to report a parcel hop");

            Parcel wrapParcel = new Parcel(3, new(), new(), trackingId, null, null, Parcel.StateEnum.InTransportEnum);

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

            Parcel parcel = new();
            Hop hop = new();

            try
            {
                DataAccess.Entities.Parcel DAparcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
                parcel = this._mapper.Map<BusinessLogic.Entities.Parcel>(DAparcel);

                DataAccess.Entities.Hop DAhop = _hopRepo.GetSingleHopByCode(code);
                hop = this._mapper.Map<BusinessLogic.Entities.Hop>(DAhop);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = null;
                if (ex.Method == "GetSingleParcelByTrackingId")
                {
                    errorMessage = $"Parcel with trackingid {trackingId} cannot be found";
                }
                else if (ex.Method == "GetSingleHopArrivalByCode")
                {
                    errorMessage = $"Hop with code {code} cannot be found";
                }

                _logger.LogError(errorMessage);
                throw new LogicDataNotFoundException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred reporting a parcel hop with trackingid {trackingId} and code {code}";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred reporting a parcel hop with trackingid {trackingId} and code {code}";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage, ex);
            }

            int futureHopsCount = parcel.FutureHops.Count();
            int visitedHopsCount = parcel.VisitedHops.Count();

            foreach (HopArrival h in parcel.FutureHops)
            {
                if (h.Code == hop.Code)
                {
                    parcel.FutureHops.Remove(h);
                    parcel.VisitedHops.Add(h);
                    break;
                }
            }

            if (futureHopsCount == parcel.FutureHops.Count() && visitedHopsCount == parcel.VisitedHops.Count())
            {
                string errorMessage = $"Cannot report the parcel hop with trackingid {trackingId} and code {code}";
                _logger.LogError(errorMessage);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage);
            }
            else if (futureHopsCount + 1 == parcel.FutureHops.Count() && visitedHopsCount - 1 == parcel.VisitedHops.Count())
            {
                _logger.LogInformation("Reported the parcel arrival at hop");
            }

            if (hop.GetType() == typeof(Warehouse))
            {
                parcel.State = Parcel.StateEnum.InTransportEnum;
            }
            else if (hop.GetType() == typeof(Truck))
            {
                parcel.State = Parcel.StateEnum.InTruckDeliveryEnum;
            }
            else if (hop.GetType() == typeof(TransferWarehouse))
            {
                TransferWarehouse warehouse = (TransferWarehouse)hop;
                //CallLogisticsPartnerAPI(warehouse, parcel);

                parcel.State = Parcel.StateEnum.TransferredEnum;
            }

            try
            {
                _webhookLogic.NotifySubscribers(parcel);

                DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _parcelRepo.Update(DAparcel);
            }
            catch (LogicDataNotFoundException)
            {
                string errorMessage = $"Webhooks with trackingId {parcel.TrackingId} cannot be found";
                _logger.LogError(errorMessage);
                throw new LogicDataNotFoundException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred updating the parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred updating the parcel with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(StaffLogic), nameof(ReportParcelHop), errorMessage, ex);
            }

            return;
        }

        public async void CallLogisticsPartnerAPI(TransferWarehouse warehouse, Parcel parcel)
        {
            HttpClient httpClient = new();
            var dataJson = JsonConvert.SerializeObject(parcel);
            var data = new StringContent(dataJson, Encoding.UTF8, "application/json");
            await httpClient.PostAsync($"{warehouse.LogisticsPartnerUrl}/parcel/{parcel.TrackingId}/", data);
        }
    }
}