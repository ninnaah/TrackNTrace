using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Parcel
    {
        public Parcel()
        {

        }
        public Parcel(int id, float weight, Recipient recipient, Recipient sender, string trackingId, List<HopArrival> visitedHops, List<HopArrival> futureHops, StateEnum state)
        {
            Id = id;
            Weight = weight;
            Recipient = recipient;
            Sender = sender;
            TrackingId = trackingId;
            VisitedHops = visitedHops;
            FutureHops = futureHops;
            State = state;
        }
        public int Id { get; set; }
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
