using Juna.SKS.Package.DataAccess.Entities;
using Juna.SKS.Package.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Sql
{
    public class SqlRecipientRepository : IRecipientRepository
    {
        public int Create(Recipient recipient)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            context.Add(recipient);

            context.SaveChanges();

            return recipient.Id;
        }

        public void Delete(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            Recipient recipient = GetSingleRecipientById(id);

            context.Remove(recipient);

            context.SaveChanges();

            return;
        }
        public void Update(Recipient recipient)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            context.Update(recipient);

            context.SaveChanges();

            return;
        }

        public IEnumerable<Recipient> GetByPostalCode(string postalCode)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Recipients.Where(p => p.PostalCode == postalCode).ToList();
        }

        public Recipient GetSingleRecipientById(int id)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Recipients.Single(p => p.Id == id) as Recipient;
        }

        public Recipient GetSingleRecipientByName(string name)
        {
            using DBContext context = new();
            context.Database.EnsureCreated();
            return context.Recipients.Single(p => p.Name == name) as Recipient;
        }

    }
}
