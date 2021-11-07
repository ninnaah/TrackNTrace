using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        public int Create(Parcel parcel)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();

            context.Add(parcel);

            context.SaveChanges();

            return parcel.Id;
        }

        public void Delete(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            Parcel parcel = GetSingleParcelById(id);

            context.Remove(parcel);

            context.SaveChanges();

            return;
        }

        public void Update(Parcel parcel)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            context.Update(parcel);

            context.SaveChanges();

            return;
        }



        public IEnumerable<Parcel> GetByRecipient(Recipient recipient)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Parcels.Where(p => p.Recipient == recipient).ToList();
        }
        public Parcel GetSingleParcelById(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Parcels.Single(p => p.Id == id) as Parcel;
        }
        public Parcel GetSingleParcelByTrackingId(string trackingid)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Parcels.Single(p => p.TrackingId == trackingid) as Parcel;
        }

    }
}
