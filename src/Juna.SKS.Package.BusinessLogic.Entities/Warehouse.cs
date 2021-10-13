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

        public Warehouse(int level, List<Hop> nextHops)
        {
            Level = level;
            NextHops = nextHops;
        }

        public Warehouse(int level, List<Hop> nextHops, string hopType, string code, string description, int processingDelayMins, string locationName, double lat, double lon, int traveltimeMins)
        {
            Level = level;
            NextHops = nextHops;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            Lat = lat;
            Lon = lon;
            TraveltimeMins = traveltimeMins;
        }
        public int? Level { get; set; }

        public List<Hop> NextHops { get; set; }
    }
}
