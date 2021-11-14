using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlHopRepository : IHopRepository
    {
        private DBContext _context;
        private readonly ILogger<SqlHopRepository> _logger;
        public SqlHopRepository(DBContext context, ILogger<SqlHopRepository> logger)
        {
            _context = context;
            _context.Database.Migrate();
            _logger = logger;
        }

        public int Create(Hop hop)
        {
            _logger.LogInformation("Trying to create hop");
            _context.Add(hop);
            _context.SaveChanges();
            _logger.LogInformation("Created hop");
            return hop.Id;
        }

        public void Update(Hop hop)
        {
            _logger.LogInformation("Trying to update hop");
            try
            {
                _context.Update(hop);
                _context.SaveChanges();
                _logger.LogInformation("Updated hop");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hop not found: {ex.Message}");
                throw;
            }

        }

        public void Delete(int id)
        {
            _logger.LogInformation("Trying to delete hop");
            Hop hop = GetSingleHopById(id);

            if (hop == null)
            {
                _logger.LogInformation("Hop not found - already deleted");
                return;
            }

            _context.Remove(hop);
            _context.SaveChanges();
            _logger.LogInformation("Deleted hop");
            return;
        }


        /*public IEnumerable<Hop> GetHopsByHopType(string hopType)
        {
            try
            {
                return _context.Hops.Where(p => p.HopType == hopType).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hops not found exception: " + ex.Message);
                return null;
            }
        }

        public Hop GetSingleHopByCode(string code)
        {
            try
            {
                return _context.Hops.Single(p => p.Code == code);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Hop not found exception: " + ex.Message);
                return null;
            }
        }*/
        public HopArrival GetSingleHopArrivalByCode(string code)
        {
            _logger.LogInformation("Trying to get single hop by code");
            try
            {
                var hop =  _context.HopArrivals.Single(p => p.Code == code);
                _logger.LogInformation("Got hop by code");
                return hop;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hop not found: {ex.Message}");
                throw;
            }
        }

        public Warehouse GetSingleWarehouseByCode(string code)
        {
            _logger.LogInformation("Trying to get single warehouse by code");
            try
            {
                var warehouse =  _context.Warehouses.Single(p => p.Code == code);
                _logger.LogInformation("Got warehouse by code");
                return warehouse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Warehouse not found: {ex.Message}");
                throw;
            }
        }

        public Hop GetSingleHopById(int id)
        {
            _logger.LogInformation("Trying to get single hop by id");
            try
            {
                var hop =  _context.Hops.Single(p => p.Id == id);
                _logger.LogInformation("Got hop by id");
                return hop;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Hop not found: {ex.Message}");
                throw;
            }
        }

        /*public IEnumerable<Warehouse> GetAllWarehouses()
        {
            try
            {
                return _context.Warehouses.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("No warehouses found exception: " + ex.Message);
                return null;
            }
        }*/

    }
}
