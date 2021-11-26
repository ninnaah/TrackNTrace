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
                _context.Update(parcel);
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

        public void Delete(int id)
        {
            _logger.LogInformation("Trying to delete parcel");
            try
            {
                Parcel parcel = GetSingleParcelById(id);

                if (parcel == null)
                {
                    _logger.LogError($"Cannot delete parcel with id {id} - already deleted");
                    throw new DataNotFoundException(nameof(SqlParcelRepository), nameof(Delete));
                }

                _context.Remove(parcel);
                _context.SaveChanges();
                _logger.LogInformation("Deleted parcel");
                return;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while deleting a parcel with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Delete), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while deleting a parcel with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(Delete), errorMessage, ex);
            }
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
                if (parcel == null)
                {
                    _logger.LogError($"Parcel with id {id} not found");
                    throw new DataNotFoundException(nameof(SqlParcelRepository), nameof(GetSingleParcelById));
                }
                _logger.LogInformation("Got parcel by id");
                return parcel;
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                string errorMessage = $"An error occured while fetching a parcel with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(GetSingleParcelById), errorMessage, ex);
            }
            catch (Exception ex)
            {
                string errorMessage = $"An unknown error occured while fetching a parcel with id {id}";
                _logger.LogError(errorMessage, ex);
                throw new DataException(nameof(SqlParcelRepository), nameof(GetSingleParcelById), errorMessage, ex);
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
