using Juna.SKS.Package.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Interfaces
{
    public interface IHopRepository
    {
        int Create(Hop hop);
        void Delete(int id);
        void DropDatabase();

        IEnumerable<Hop> GetHopsByHopType(string hopType);
        HopArrival GetSingleHopArrivalByCode(string code);
        Hop GetSingleHopByCode(string code);
        Hop GetSingleHopById(int id);
        Warehouse GetWarehouseHierarchy();
    }
}
