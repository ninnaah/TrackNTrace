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
        public Hop(string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public string HopType { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int? ProcessingDelayMins { get; set; }

        public string LocationName { get; set; }

        public GeoCoordinate LocationCoordinates { get; set; }
    }
}
