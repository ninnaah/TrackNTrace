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

        /*public DBContext(DbContextOptions<DBContext> options) : base(options)
        { 
        
        }*/

        public DbSet<Hop> Hops { get; set; }
        public DbSet<GeoCoordinate> GeoCoordinates { get; set; }
        public DbSet<Transferwarehouse> Transferwarehouses { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WarehouseNextHops> WarehouseNextHops { get; set; }
        public DbSet<Parcel> Parcels { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<HopArrival> HopArrivals { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
             optionsBuilder.UseSqlServer(@"Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;");
        }

    }


}
