using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Interfaces
{
    public interface IWebhookRepository
    {
        long Create(WebhookResponse response);
        void Update(WebhookResponse response);
        void Delete(long? id);
        WebhookResponses GetWebhookResponsesByTrackingId(string trackingId);
        WebhookResponse GetSingleWebhookResponseById(long? id);
    }
}
