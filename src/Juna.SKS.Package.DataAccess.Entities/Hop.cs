using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Hop
    {
        public Hop()
        {
        }
        public Hop(int id, string hopType, string code, string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            Id = id;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public int Id { get; set; }
        public string HopType { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public int? ProcessingDelayMins { get; set; }

        public string LocationName { get; set; }

        public GeoCoordinate LocationCoordinates { get; set; }
    }
}
