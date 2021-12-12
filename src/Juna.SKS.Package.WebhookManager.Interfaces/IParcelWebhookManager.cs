using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.WebhookManager.Interfaces
{
    public interface IParcelWebhookManager
    {
        WebhookResponses ListParcelWebhooks(string trackingId);
        WebhookResponse SubscribeParcelWebhook(string trackingId, string url);
        void UnsubscribeParcelWebhook(long? id);
        void NotifySubscribers(Parcel parcel);

    }
}
