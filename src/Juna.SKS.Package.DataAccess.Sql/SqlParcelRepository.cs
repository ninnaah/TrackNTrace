using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Juna.SKS.Package.DataAccess.Interfaces.Exceptions;
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
            _context.Database.EnsureCreated();
            _logger = logger;
        }
        public int Create(Parcel parcel)
        {
            _logger.LogInformation("Trying to create parcel");
            try
            {
                _context.Add(parcel);
                _context.SaveChanges();
                _logger.LogInformation("Created parcel");
                return parcel.Id;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while creating a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Create), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while creating a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Create), errorMessage, ex);
            }
        }

        public void Update(Parcel parcel)
        {
            _logger.LogInformation("Trying to update parcel");
            try
            {
                Parcel oldParcel = GetSingleParcelByTrackingId(parcel.TrackingId);

                if (oldParcel == null)
                {
                    _logger.LogError($"Cannot update parcel with id {parcel.Id} - not found");
                    throw new DataNotFoundException(nameof(SqlParcelRepository), nameof(Update));
                }
                _context.Remove(oldParcel.Recipient);
                _context.Remove(oldParcel.Sender);

                foreach(HopArrival hop in oldParcel.FutureHops)
                    _context.Remove(hop);

                foreach (HopArrival hop in oldParcel.VisitedHops)
                    _context.Remove(hop);

                _context.Remove(oldParcel);
                _context.SaveChanges();

                _context.Add(parcel);
                _context.SaveChanges();
                _logger.LogInformation("Updated parcel");
                return;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while updating a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Update), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while updating a parcel with trackingid {parcel.TrackingId}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Update), errorMessage, ex);
            }
        }


        public Parcel GetSingleParcelByTrackingId(string trackingid)
        {
            _logger.LogInformation("Trying to get single parcel by tracking id");
            try
            {
                var parcel = _context.Parcels.Single(p => p.TrackingId == trackingid);
                if (parcel == null)
                {
                    _logger.LogError($"Parcel with trackingid {trackingid} not found");
                    throw new DataNotFoundException(nameof(SqlParcelRepository), nameof(GetSingleParcelByTrackingId));
                }
                _logger.LogInformation("Got parcel by tracking id");
                return parcel;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a parcel with trackingid {trackingid}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(GetSingleParcelByTrackingId), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a parcel with trackingid {trackingid}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(GetSingleParcelByTrackingId), errorMessage, ex);
            }
        }

    }
}
