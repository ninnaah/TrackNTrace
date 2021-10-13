using Juna.SKS.Package.BusinessLogic.Entities;
using Juna.SKS.Package.BusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic
{
    public class RecipientLogic : IRecipientLogic
    {
        public RecipientLogic()
        {

        }

        public Parcel TrackParcel(string trackingId)
        {
            Recipient recipient = new Recipient("Tom", "Examplestreet", "1220", "Vienna", "Austira");
            Recipient sender = new Recipient("Jerry", "Examplestreet", "1220", "Vienna", "Austira");
            List<HopArrival> visitedHops = new List<HopArrival>
            {
                new HopArrival("111", "aVisitedHop", DateTime.Now),
                new HopArrival("222", "anotherVisitedHop", DateTime.Now)
            };
            List<HopArrival> futureHops = new List<HopArrival>
            {
                new HopArrival("333", "aFutureHop", DateTime.Now),
                new HopArrival("444", "anotherFutureHop", DateTime.Now)
            };

            Parcel parcel = new Parcel(3, recipient, sender, trackingId, visitedHops, futureHops);
            return parcel;
        }
    }
}
