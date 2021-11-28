using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class GeoCoordinate
    {
        public GeoCoordinate()
        {
                
        }

        public GeoCoordinate(int id, double lat, double lon)
        {
            Id = id;
            Lat = lat;
            Lon = lon;
        }
        public int Id { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }
    }
}
