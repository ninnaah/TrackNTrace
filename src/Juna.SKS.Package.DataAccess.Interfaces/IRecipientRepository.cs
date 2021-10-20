using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Interfaces
{
    public interface IRecipientRepository
    {
        int Create(Recipient recipient);
        void Update(Recipient recipient);
        void Delete(int id);

        IEnumerable<Recipient> GetByPostalCode(string postalCode); //??
        Recipient GetById(int id);
        Recipient GetByName(string name);
    }
}
