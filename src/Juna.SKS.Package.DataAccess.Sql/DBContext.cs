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
        public virtual DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        public virtual DbSet<Truck> Trucks { get; set; }
        public virtual DbSet<Warehouse> Warehouses { get; set; }
        public virtual DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        public virtual DbSet<Parcel> Parcels { get; set; }
        public virtual DbSet<Recipient> Recipients { get; set; }
        public virtual DbSet<HopArrival> HopArrivals { get; set; }


    }


}
