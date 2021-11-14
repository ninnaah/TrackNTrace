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
        void Update(Hop hop);
        void Delete(int id);

        /*IEnumerable<Hop> GetHopsByHopType(string hopType);
        Hop GetSingleHopByCode(string code);*/
        HopArrival GetSingleHopArrivalByCode(string code);
        Warehouse GetSingleWarehouseByCode(string code);
        Hop GetSingleHopById(int id);
        
        //IEnumerable<Warehouse> GetAllWarehouses();

    }
}
