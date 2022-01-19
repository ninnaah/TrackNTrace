using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHops
    {
        public WarehouseNextHops()
        {

        }

        public WarehouseNextHops(int id, int travelMin, Hop hop)
        {
            Id = id;
            TraveltimeMins = travelMin;
            Hop = hop;
        }

        public int Id { get;  set; }
        public int? TraveltimeMins { get; set; }
        public Hop Hop { get; set; }
        public Warehouse Parent { get; set; }
    }
}
