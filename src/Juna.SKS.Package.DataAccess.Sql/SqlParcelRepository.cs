using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlParcelRepository : IParcelRepository
    {
        private DBContext _context;
        private readonly ILogger<SqlParcelRepository> _logger;
        public SqlParcelRepository(DBContext context, ILogger<SqlParcelRepository> logger)
        {
            _context = context;
            _context.Database.Migrate();
            _logger = logger;
        }
        public int Create(Parcel parcel)
        {
            _logger.LogInformation("Trying to create parcel");
            _context.Add(parcel);
            _context.SaveChanges();
            _logger.LogInformation("Created parcel");
            return parcel.Id;
        }

        public void Update(Parcel parcel)
        {
            _logger.LogInformation("Trying to update parcel");
            try
            {
                _context.Update(parcel);
                _context.SaveChanges();
                _logger.LogInformation("Updated parcel");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Parcel not found: {ex.Message}");
                throw;
            }
        }

        public void Delete(int id)
        {
            _logger.LogInformation("Trying to delete parcel");
            Parcel parcel = GetSingleParcelById(id);

            if (parcel == null)
            {
                _logger.LogInformation("Parcel not found - already deleted");
                return;
            }

            _context.Remove(parcel);
            _context.SaveChanges();
            _logger.LogInformation("Deleted parcel");
            return;
        }

        

        /*public IEnumerable<Parcel> GetParcelsByRecipient(Recipient recipient)
        {
            try
            {
                return _context.Parcels.Where(p => p.Recipient == recipient).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Parcels not found exception: " + ex.Message);
                return null;
            }
        }*/
        public Parcel GetSingleParcelById(int id)
        {
            _logger.LogInformation("Trying to get single parcel by id");
            try
            {
                var parcel =  _context.Parcels.Single(p => p.Id == id);
                _logger.LogInformation("Got parcel by id");
                return parcel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Parcel not found: {ex.Message}");
                throw;
            }

        }
        public Parcel GetSingleParcelByTrackingId(string trackingid)
        {
            _logger.LogInformation("Trying to get single parcel by tracking id");
            try
            {
                var parcel = _context.Parcels.Single(p => p.TrackingId == trackingid);
                _logger.LogInformation("Got parcel by tracking id");
                return parcel;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Parcel not found: {ex.Message}");
                throw;
            }
        }

    }
}
