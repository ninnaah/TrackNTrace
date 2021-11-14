using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Sql
{
    /*public class SqlRecipientRepository : IRecipientRepository
    {
        private DBContext _context;
        public SqlRecipientRepository(DBContext context)
        {
            _context = context;
            _context.Database.Migrate();
        }
        public int Create(Recipient recipient)
        {
            _context.Add(recipient);

            _context.SaveChanges();

            return recipient.Id;
        }

        public void Delete(int id)
        {
            Recipient recipient = GetSingleRecipientById(id);

            if (recipient == null)
                return;

            _context.Remove(recipient);

            _context.SaveChanges();

            return;
        }
        public void Update(Recipient recipient)
        {
            _context.Update(recipient);

            _context.SaveChanges();

            return;
        }

        public IEnumerable<Recipient> GetRecipientsByPostalCode(string postalCode)
        {
            try
            {
                return _context.Recipients.Where(p => p.PostalCode == postalCode).ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Recipients not found exception: " + ex.Message);
                return null;
            }
        }

        public Recipient GetSingleRecipientById(int id)
        {
            try
            {
                return _context.Recipients.Single(p => p.Id == id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Recipient not found exception: " + ex.Message);
                return null;
            }
        }

        public Recipient GetSingleRecipientByName(string name)
        {
            try
            {
                return _context.Recipients.Single(p => p.Name == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Recipient not found exception: " + ex.Message);
                return null;
            }
        }

    }*/
}
