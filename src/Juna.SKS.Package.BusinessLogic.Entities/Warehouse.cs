using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Warehouse : Hop
    {
        public Warehouse()
        {

        }

        public Warehouse(int level, List<WarehouseNextHops> nextHops, string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            Level = level;
            NextHops = nextHops;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public int? Level { get; set; }

        public List<WarehouseNextHops> NextHops { get; set; }
    }
}
