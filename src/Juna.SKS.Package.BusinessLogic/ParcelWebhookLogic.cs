using AutoMapper;
using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Juna.SKS.Package.WebhookManager.Interfaces;
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
        private readonly ILogger<ParcelWebhookLogic> _logger;
        private readonly IParcelWebhookManager _webhookManager;

        public ParcelWebhookLogic(IMapper mapper, ILogger<ParcelWebhookLogic> logger, IParcelWebhookManager webhookManager)
        {
            _mapper = mapper;
            _logger = logger;
            _webhookManager = webhookManager;
        }

        public WebhookResponses ListParcelWebhooks(string trackingId)
        {
            _logger.LogInformation("Trying to list parcel webhooks");

            WebhookResponses webhooks;
            try
            {
                DataAccess.Entities.WebhookResponses DAwebhooks = _webhookManager.ListParcelWebhooks(trackingId);
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
                string errorMessage = $"An error occurred listing webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(ListParcelWebhooks), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred listing webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(ListParcelWebhooks), errorMessage, ex);
            }

            _logger.LogInformation("Listed parcel webhooks");
            return webhooks;
        }

        public WebhookResponse SubscribeParcelWebhook(string trackingId, string url)
        {
            _logger.LogInformation("Trying to subscribe a parcel webhook");

            WebhookResponse webhook;
            try
            {
                DataAccess.Entities.WebhookResponse DAwebhook = _webhookManager.SubscribeParcelWebhook(trackingId, url);
                webhook = this._mapper.Map<WebhookResponse>(DAwebhook);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Parcel cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred subscribing for a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred subscribing for a parcel with trackingId {trackingId}";
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
                _webhookManager.UnsubscribeParcelWebhook(id);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhook cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred unsubscribing a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred unsubscribing a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            
            _logger.LogInformation("Unubscribed a parcel webhook");
            return;
        }

        public void NotifySubscribers(Parcel parcel)
        {
            _logger.LogInformation("Trying to notify subscribers");

            try
            {
                DataAccess.Entities.Parcel DAparcel = this._mapper.Map<DataAccess.Entities.Parcel>(parcel);
                _webhookManager.NotifySubscribers(DAparcel);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhook cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new LogicDataNotFoundException(nameof(ParcelWebhookLogic), nameof(NotifySubscribers), errorMessage);
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(NotifySubscribers), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new LogicException(nameof(ParcelWebhookLogic), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }

            _logger.LogInformation("Notified subscribers");
            return;
        }
    }
}
