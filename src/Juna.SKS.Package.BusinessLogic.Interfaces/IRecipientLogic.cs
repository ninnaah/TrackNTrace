using Juna.SKS.Package.BusinessLogic.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface IRecipientLogic
    {
        Parcel TrackParcel(string trackingId);
    }
}
