using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.WebhookManager.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Juna.SKS.Package.WebhookManager.Interfaces.Exceptions;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;

namespace Juna.SKS.Package.WebhookManager
{
    public class ParcelWebhookManager : IParcelWebhookManager
    {
        private readonly IWebhookRepository _webhookRepo;
        private readonly IParcelRepository _parcelRepo;
        private readonly ILogger<ParcelWebhookManager> _logger;
        private HttpClient _client;

        public ParcelWebhookManager(IWebhookRepository webhookRepo, IParcelRepository parcelRepo, ILogger<ParcelWebhookManager> logger, HttpClient client)
        {
            _logger = logger;
            _webhookRepo = webhookRepo;
            _parcelRepo = parcelRepo;
            _client = client;
        }

        public WebhookResponses ListParcelWebhooks(string trackingId)
        {
            _logger.LogInformation("Trying to list parcel webhooks");

            WebhookResponses webhooks;
            try
            {
                webhooks = _webhookRepo.GetWebhookResponsesByTrackingId(trackingId);
            }
            catch (DataAccess.Interfaces.Exceptions.DataNotFoundException ex)
            {
                string errorMessage = $"Webhooks with trackingId {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataNotFoundException(nameof(ParcelWebhookManager), nameof(ListParcelWebhooks));
            }
            catch (DataAccess.Interfaces.Exceptions.DataException ex)
            {
                string errorMessage = $"An error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(ListParcelWebhooks), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(ListParcelWebhooks), errorMessage, ex);
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
            catch (DataAccess.Interfaces.Exceptions.DataNotFoundException ex)
            {
                string errorMessage = $"Parcel cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataNotFoundException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook));
            }
            catch (DataAccess.Interfaces.Exceptions.DataException ex)
            {
                string errorMessage = $"An error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }

            WebhookResponse webhook = new(trackingId, url, DateTime.Now);

            try
            {
                var webhookId = _webhookRepo.Create(webhook);
                webhook.Id = webhookId;
            }
            catch (DataAccess.Interfaces.Exceptions.DataException ex)
            {
                string errorMessage = $"An error occurred creating a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred creating a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
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
            catch (DataAccess.Interfaces.Exceptions.DataNotFoundException ex)
            {
                string errorMessage = $"Webhook cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataNotFoundException(nameof(ParcelWebhookManager), nameof(UnsubscribeParcelWebhook));
            }
            catch (DataAccess.Interfaces.Exceptions.DataException ex)
            {
                string errorMessage = $"An error occurred deleting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(UnsubscribeParcelWebhook), errorMessage, ex); ;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred deliting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            
            _logger.LogInformation("Unubscribed a parcel webhook");
            return;
        }

        public void NotifySubscribers(Parcel parcel)
        {
            _logger.LogInformation("Trying to notify subscribers");
            WebhookResponses webhooks = new();
            try
            {
                webhooks = ListParcelWebhooks(parcel.TrackingId);
            }
            catch (Interfaces.Exceptions.DataNotFoundException ex)
            {
                string errorMessage = $"Webhooks with trackingId {parcel.TrackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataNotFoundException(nameof(ParcelWebhookManager), nameof(NotifySubscribers));
            }
            catch (Interfaces.Exceptions.DataException ex)
            {
                string errorMessage = $"An error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(NotifySubscribers), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new Interfaces.Exceptions.DataException(nameof(ParcelWebhookManager), nameof(NotifySubscribers), errorMessage, ex);
            }

            var json = JsonConvert.SerializeObject(parcel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            foreach (WebhookResponse webhook in webhooks)
            {
                var response = _client.PostAsync(webhook.Url, data);
                _logger.LogInformation($"Got response to POST request from webhook with URL {webhook.Url}");
            }

            _logger.LogInformation("Notified subscribers");
        }


    }
}
