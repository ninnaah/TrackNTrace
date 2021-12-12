using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WebhookResponse
    {
        public WebhookResponse()
        {

        }
        public WebhookResponse(string trackingId, string url, DateTime createdAt)
        {
            TrackingId = trackingId;
            Url = url;
            CreatedAt = createdAt;
        }

        public WebhookResponse(long id, string trackingId, string url, DateTime createdAt)
        {
            Id = id;
            TrackingId = trackingId;
            Url = url;
            CreatedAt = createdAt;
        }
        public long Id { get; set; }

        public string TrackingId { get; set; }

        public string Url { get; set; }

        public DateTime? CreatedAt { get; set; }

    }
}
