using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using AutoMapper;
using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.WebhookManager.Interfaces;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Juna.SKS.Package.WebhookManager
{
    public class ParcelWebhookManager : IParcelWebhookManager
    {
        private readonly IWebhookRepository _webhookRepo;
        private readonly IParcelRepository _parcelRepo;
        private readonly ILogger<ParcelWebhookManager> _logger;

        public ParcelWebhookManager(IWebhookRepository webhookRepo, IParcelRepository parcelRepo, ILogger<ParcelWebhookManager> logger)
        {
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
                webhooks = _webhookRepo.GetWebhookResponsesByTrackingId(trackingId);
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhooks with trackingId {trackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(ParcelWebhookManager), nameof(ListParcelWebhooks), errorMessage, ex);
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
                throw;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred fetching a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
            }

            WebhookResponse webhook = new(trackingId, url, DateTime.Now);

            try
            {
                var webhookId = _webhookRepo.Create(webhook);
                webhook.Id = webhookId;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred creating a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred creating a parcel with trackingId {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(ParcelWebhookManager), nameof(SubscribeParcelWebhook), errorMessage, ex);
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
                throw;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred deleting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred deliting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(ParcelWebhookManager), nameof(UnsubscribeParcelWebhook), errorMessage, ex);
            }
            
            _logger.LogInformation("Unubscribed a parcel webhook");
            return;
        }

        public async void NotifySubscribers(Parcel parcel)
        {
            _logger.LogInformation("Trying to notify subscribers");

            var json = JsonConvert.SerializeObject(parcel);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var webhooks = ListParcelWebhooks(parcel.TrackingId);

                using var client = new HttpClient();
                HttpResponseMessage response = new HttpResponseMessage();

                foreach (WebhookResponse webhook in webhooks)
                {
                    response = await client.PostAsync(webhook.Url, data);
                    string result = response.Content.ReadAsStringAsync().Result;
                    _logger.LogInformation($"Got response to POST request from webhook with URL {webhook.Url}");
                }
                
            }
            catch (DataNotFoundException ex)
            {
                string errorMessage = $"Webhooks with trackingId {parcel.TrackingId} cannot be found";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (DataException ex)
            {
                string errorMessage = $"An error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw;
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occurred notifying webhooks with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(ParcelWebhookManager), nameof(NotifySubscribers), errorMessage, ex);
            }

            _logger.LogInformation("Notified subscribers");
        }


    }
}
