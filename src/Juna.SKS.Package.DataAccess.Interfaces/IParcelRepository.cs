using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace Juna.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        int Create(Parcel parcel);
        void Update(Parcel parcel);
        void Delete(int id);
        
        IEnumerable<Parcel> GetByRecipient(Recipient recipient); //??
        Parcel GetById(int id);
        Parcel GetByTrackingId(string trackingid);
    }
}
