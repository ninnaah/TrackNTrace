using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Parcel
    {
        public float? Weight { get; set; }
        public Recipient Recipient { get; set; }
        public Recipient Sender { get; set; }
        public string TrackingId { get; set; }
        public List<HopArrival> VisitedHops { get; set; }
        public List<HopArrival> FutureHops { get; set; }
    }
}
