using Juna.SKS.Package.BusinessLogic.Entities;
using System;

namespace Juna.SKS.Package.BusinessLogic.Interfaces
{
    public interface ILogisticsPartnerLogic
    {
        string TransitionParcel(Parcel parcel, string trackingId);
    }
}
