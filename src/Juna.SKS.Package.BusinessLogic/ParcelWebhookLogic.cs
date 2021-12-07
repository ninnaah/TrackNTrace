using AutoMapper;
using Juna.SKS.Package.BusinessLogic.Entities;
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
    public class ParcelWebhookLogic : IParcelWebhookLogic
    {
        private readonly IMapper _mapper;
        private readonly IWebhookRepository _webhookRepo;
        private readonly IParcelRepository _parcelRepo;
        private readonly ILogger<ParcelWebhookLogic> _logger;

        public ParcelWebhookLogic(IMapper mapper, IWebhookRepository webhookRepo, IParcelRepository parcelRepo, ILogger<ParcelWebhookLogic> logger)
        {
            _mapper = mapper;
            _logger = logger;
            _webhookRepo = webhookRepo;
            _parcelRepo = parcelRepo;
        }

        public WebhookResponses ListParcelWebhooks(string trackingId)
        {
            _logger.LogInformation("Trying to list parcel webhooks");

            WebhookResponses webhooks;
            try
            {
                DataAccess.Entities.WebhookResponses DAwebhooks = _webhookRepo.GetWebhookResponsesByTrackingId(trackingId);
                webhooks = this._mapper.Map<WebhookResponses>(DAwebhooks);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhooks with trackingId {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(ListParcelWebhooks), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(ListParcelWebhooks), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(ListParcelWebhooks), errorMessage, ex);
            }

            _logger.LogInformation("Listed parcel webhooks");
            return webhooks;
        }

        public WebhookResponse SubscribeParcelWebhook(string trackingId, string url)
        {
            _logger.LogInformation("Trying to subscribe a parcel webhook");


            try
            {
                var parcel = _parcelRepo.GetSingleParcelByTrackingId(trackingId);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Parcel cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }

            WebhookResponse webhook = new(trackingId, url, DateTime.Now);

            try
            {
                DataAccess.Entities.WebhookResponse DAwebhook = this._mapper.Map<DataAccess.Entities.WebhookResponse>(webhook);
                var webhookId = _webhookRepo.Create(DAwebhook);
                webhook.Id = webhookId;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }

            _logger.LogInformation("Subscribed a parcel webhook");
            return webhook;
        }

        public void UnsubscribeParcelWebhook(long? id)
        {
            _logger.LogInformation("Trying to unsubscribe a parcel webhook");

            try
            {
                _webhookRepo.Delete(id);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhook cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred deleting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred deliting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            
            _logger.LogInformation("Unubscribed a parcel webhook");
            return;
        }
    }
}
