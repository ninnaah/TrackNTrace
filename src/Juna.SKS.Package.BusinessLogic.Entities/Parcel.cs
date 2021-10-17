using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Parcel
    {
        public Parcel()
        {

        }
        public Parcel(float weight, Recipient recipient, Recipient sender, string trackingId, List<HopArrival> visitedHops, List<HopArrival> futureHops, StateEnum state)
        {
            Weight = weight;
            Recipient = recipient;
            Sender = sender;
            TrackingId = trackingId;
            VisitedHops = visitedHops;
            FutureHops = futureHops;
            State = state;
        }
        public float? Weight { get; set; }
        public Recipient Recipient { get; set; }
        public Recipient Sender { get; set; }
        public string TrackingId { get; set; }
        public List<HopArrival> VisitedHops { get; set; }
        public List<HopArrival> FutureHops { get; set; }

        public enum StateEnum
        {
            PickupEnum = 0,
            InTransportEnum = 1,
            InTruckDeliveryEnum = 2,
            TransferredEnum = 3,
            DeliveredEnum = 4
        }
        public StateEnum? State { get; set; }
    }
}
