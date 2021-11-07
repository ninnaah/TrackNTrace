using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        public int Create(Hop hop)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            context.Add(hop);

            context.SaveChanges();

            return hop.Id;
        }

        public void Delete(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            Hop hop = GetSingleHopById(id);

            context.Remove(hop);

            context.SaveChanges();

            return;
        }

        public void Update(Hop hop)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            context.Update(hop);

            context.SaveChanges();

            return;
        }

        public IEnumerable<Hop> GetByHopType(string hopType)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Hops.Where(p => p.HopType == hopType).ToList();
        }

        public Hop GetSingleHopByCode(string code)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Hops.Single(p => p.Code == code) as Hop;
        }
        public HopArrival GetSingleHopArrivalByCode(string code)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.HopArrivals.Single(p => p.Code == code) as HopArrival;
        }

        public Warehouse GetSingleWarehouseByCode(string code)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Warehouses.Single(p => p.Code == code) as Warehouse;
        }

        public Hop GetSingleHopById(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Hops.Single(p => p.Id == id) as Hop;
        }

    }
}
