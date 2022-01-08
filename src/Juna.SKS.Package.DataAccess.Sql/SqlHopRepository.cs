using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
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
            _context.Database.EnsureCreated();
            _logger = logger;
        }

        public int Create(Hop hop)
        {
            _logger.LogInformation("Trying to create hop");
            try
            {
                _context.Add(hop);
                _context.SaveChanges();
                _logger.LogInformation("Created hop");
                return hop.Id;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while creating a hop with code {hop.Code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(Create), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while creating a hop with code {hop.Code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(Create), errorMessage, ex);
            }
        }

        public void Delete(int id)
        {
            _logger.LogInformation("Trying to delete hop");
            try
            {
                Hop hop = GetSingleHopById(id);

                if (hop == null)
                {
                    _logger.LogError($"Cannot delete hop with id {id} - already deleted");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(Delete));
                }

                _context.Remove(hop);
                _context.SaveChanges();
                _logger.LogInformation("Deleted hop");
                return;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while deleting a hop with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(Delete), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while deleting a hop with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(Delete), errorMessage, ex);
            }
        }

        public void DropDatabase()
        {
            _logger.LogInformation("Trying to drop database");
            try
            {
                _context.WarehouseNextHops.RemoveRange(_context.WarehouseNextHops);
                _context.Hops.RemoveRange(_context.Hops);
                _context.GeoCoordinates.RemoveRange(_context.GeoCoordinates);
                _context.HopArrivals.RemoveRange(_context.HopArrivals);
                _context.Parcels.RemoveRange(_context.Parcels);
                _context.Recipients.RemoveRange(_context.Recipients);

                _context.SaveChanges();
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while dropping the database";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(DropDatabase), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while dropping the database";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(DropDatabase), errorMessage, ex);
            }
            _logger.LogInformation("Dropped database");
            _logger.LogInformation("Trying to create new database");
            _context.Database.EnsureCreated();
            _context.SaveChanges();
            _logger.LogInformation("Created new database");
        }

        public IEnumerable<Hop> GetHopsByHopType(string hopType)
        {
            _logger.LogInformation($"Trying to get hops by hopType {hopType}");
            try 
            { 
                var hops = _context.Hops.Include(w => w.Parent).ThenInclude(nh => nh.Hop).ThenInclude(nh => nh.LocationCoordinates);

                if (hops == null)
                {
                    _logger.LogError($"Hops with hoptype {hopType} not found");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(GetHopsByHopType));
                }
                _logger.LogInformation($"Got hops by hopType {hopType}");

                return hops.ToList().Where(p => p.HopType == hopType);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching hops by hoptype {hopType}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetHopsByHopType), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching  hops by hoptype {hopType}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetHopsByHopType), errorMessage, ex);
            }
        }

        public HopArrival GetSingleHopArrivalByCode(string code)
        {
            _logger.LogInformation("Trying to get single hop by code");
            try
            {
                var hop =  _context.HopArrivals.Single(p => p.Code == code);
                if(hop == null)
                {
                    _logger.LogError($"Hop with code {code} not found");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(GetSingleHopArrivalByCode));
                }
                _logger.LogInformation("Got hop by code");
                return hop;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a hop with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopArrivalByCode), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a hop with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopArrivalByCode), errorMessage, ex);
            }
        }

        public Hop GetSingleHopByCode(string code)
        {
            _logger.LogInformation("Trying to get single hop by code");
            try
            {
                var hop =  _context.Hops.Include(nh => nh.LocationCoordinates).Single(p => p.Code == code);
                if (hop == null)
                {
                    _logger.LogError($"Hop with code {code} not found");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(GetSingleHopByCode));
                }
                _logger.LogInformation($"Got hop with code {code}");
                return hop;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a hop with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopByCode), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a hop with code {code}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopByCode), errorMessage, ex);
            }
        }

        public Hop GetSingleHopById(int id)
        {
            _logger.LogInformation("Trying to get single hop by id");
            try
            {
                var hop =  _context.Hops.Single(p => p.Id == id);
                if (hop == null)
                {
                    _logger.LogError($"Hop with id {id} not found");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(GetSingleHopById));
                }
                _logger.LogInformation("Got hop by id");
                return hop;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a hop with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopById), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a hop with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetSingleHopById), errorMessage, ex);
            }
        }

        public Warehouse GetWarehouseHierarchy()
        {
            _logger.LogInformation("Trying to get warehouse hierarchy");
            try
            {
                var hierarchy = _context.Warehouses.Include(w => w.NextHops).ThenInclude(nh => nh.Hop).ThenInclude(nh => nh.LocationCoordinates);

                if (hierarchy == null)
                {
                    _logger.LogError($"Warehouse hierarchy not found");
                    throw new DataNotFoundException(nameof(SqlHopRepository), nameof(GetWarehouseHierarchy));
                }
                _logger.LogInformation("Got warehouse hierarchy");
                return hierarchy.ToList().SingleOrDefault(w => w.Level == 0);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching warehous hierarchy";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetWarehouseHierarchy), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching warehous hierarchy";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlHopRepository), nameof(GetWarehouseHierarchy), errorMessage, ex);
            }
        }

    }
}
