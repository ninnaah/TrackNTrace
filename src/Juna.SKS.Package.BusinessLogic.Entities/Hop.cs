using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Hop
    {
        public Hop()
        {
        }
        public Hop(string hopType, string code, string description, int processingDelayMins, string locationName, double lat, double lon, int traveltimeMins)
        {
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            Lat = lat;
            Lon = lon;
            TraveltimeMins = traveltimeMins;
        }
        public string HopType { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int? ProcessingDelayMins { get; set; }

        public string LocationName { get; set; }

        public double? Lat { get; set; }

        public double? Lon { get; set; }
        public int? TraveltimeMins { get; set; }
    }
}
