using Juna.SKS.Package.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class DBContext : DbContext
    {
        public DBContext()
        {
                
        }
       
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { 
       
        }

        public virtual DbSet<Hop> Hops { get; set; }
        public virtual DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public virtual DbSet<TransferWarehouse> Transferwarehouses { get; set; }
        public virtual DbSet<Truck> Trucks { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        public virtual DbSet<Parcel> Parcels { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<HopArrival> HopArrivals { get; set; }
        public virtual DbSet<WebhookResponse> WebhookResponses { get; set; }


        protected override void OnModelCreating (ModelBuilder builder)
        {
            builder.Entity<Hop>().HasDiscriminator(h => h.HopType);

            builder.Entity<Truck>().HasBaseType<Hop>();
            builder.Entity<TransferWarehouse>().HasBaseType<Hop>();
            builder.Entity<Warehouse>().HasBaseType<Hop>();

            builder.Entity<Parcel>().Navigation(p => p.FutureHops).AutoInclude();
            builder.Entity<Parcel>().Navigation(p => p.VisitedHops).AutoInclude();
            builder.Entity<Parcel>().Navigation(p => p.Recipient).AutoInclude();
            builder.Entity<Parcel>().Navigation(p => p.Sender).AutoInclude();

            builder.Entity<Warehouse>().HasMany<WarehouseNextHops>(h => h.NextHops).WithOne(p => p.Parent);
            builder.Entity<WarehouseNextHops>().HasOne<Hop>(wnh => wnh.Hop).WithOne( p => p.Parent).HasForeignKey<WarehouseNextHops>(wnh => wnh.Id);
        }


    }


}
