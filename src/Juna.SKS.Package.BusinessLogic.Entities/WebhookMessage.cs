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

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    //public class WebhookMessage : TrackingInformation
    public class WebhookMessage
    {
        public string TrackingId { get; set; }

    }
}
