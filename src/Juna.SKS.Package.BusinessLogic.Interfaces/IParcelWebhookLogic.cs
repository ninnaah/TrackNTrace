using Juna.SKS.Package.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelWebhookLogic
    {
        WebhookResponses ListParcelWebhooks(string trackingId);
        WebhookResponse SubscribeParcelWebhook(string trackingId, string url);
        void UnsubscribeParcelWebhook(long? id);

    }
}
