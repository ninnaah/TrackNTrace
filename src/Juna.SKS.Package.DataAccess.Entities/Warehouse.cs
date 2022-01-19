using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Warehouse : Hop
    {
        public Warehouse()
        {

        }

        public Warehouse(int id, int level, List<WarehouseNextHops> nextHops, string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            Id = id;
            Level = level;
            NextHops = nextHops;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public Warehouse(WarehouseNextHops parent, string code, string description)
        {
            Parent = parent;
            Code = code;
            Description = description;

        }
        public int? Level { get; set; }

        public List<WarehouseNextHops> NextHops { get; set; }
    }
}
