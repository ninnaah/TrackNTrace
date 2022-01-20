using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlWebhookRepository : IWebhookRepository
    {
        private DBContext _context;
        private readonly ILogger<SqlWebhookRepository> _logger;

        public SqlWebhookRepository(DBContext context, ILogger<SqlWebhookRepository> logger)
        {
            _context = context;
            _context.Database.EnsureCreated();
            _logger = logger;
        }

        public long Create(WebhookResponse response)
        {
            _logger.LogInformation("Trying to create webhook");
            try
            {
                _context.Add(response);
                _context.SaveChanges();
                _logger.LogInformation("Created webhook");
                return response.Id;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while creating a webhook with trackingid {response.TrackingId} and url {response.Url}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(Create), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while creating a webhook with trackingid {response.TrackingId} and url {response.Url}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(Create), errorMessage, ex);
            }
        }

        public void Delete(long? id)
        {
            _logger.LogInformation("Trying to delete webhook");
            try
            {
                WebhookResponse webhook = GetSingleWebhookResponseById(id);

                if (webhook == null)
                {
                    _logger.LogError($"Cannot delete webhook with id {id} - already deleted");
                    throw new DataNotFoundException(nameof(SqlWebhookRepository), nameof(Delete));
                }

                _context.Remove(webhook);
                _context.SaveChanges();
                _logger.LogInformation("Deleted webhook");
                return;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while deleting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(Delete), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while deleting a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(Delete), errorMessage, ex);
            }
        }

        public WebhookResponses GetWebhookResponsesByTrackingId(string trackingId)
        {
            _logger.LogInformation("Trying to get webhooks by trackingid");
            try
            {
                var webhooks = _context.WebhookResponses.Where(p => p.TrackingId == trackingId).ToList();
                WebhookResponses webhookResponses = new();

                foreach(WebhookResponse webhook in webhooks)
                {
                    webhookResponses.Add(webhook);
                }
                
                if (webhooks == null)
                {
                    _logger.LogError($"Webhooks with trackingid {trackingId} not found");
                    throw new DataNotFoundException(nameof(SqlWebhookRepository), nameof(GetWebhookResponsesByTrackingId));
                }
                _logger.LogInformation("Got webhook by id");
                return webhookResponses;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(GetWebhookResponsesByTrackingId), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching webhooks with trackingid {trackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(GetWebhookResponsesByTrackingId), errorMessage, ex);
            }
        }

        public WebhookResponse GetSingleWebhookResponseById(long? id)
        {
            _logger.LogInformation("Trying to get single webhook by id");
            try
            {
                var webhook = _context.WebhookResponses.Single(p => p.Id == id);
                if (webhook == null)
                {
                    _logger.LogError($"Webhook with id {id} not found");
                    throw new DataNotFoundException(nameof(SqlWebhookRepository), nameof(GetSingleWebhookResponseById));
                }
                _logger.LogInformation("Got webhook by id");
                return webhook;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(GetSingleWebhookResponseById), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a webhook with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlWebhookRepository), nameof(GetSingleWebhookResponseById), errorMessage, ex);
            }
        }
    }
}
