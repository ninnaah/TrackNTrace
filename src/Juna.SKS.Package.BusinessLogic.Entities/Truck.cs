using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Truck : Hop
    {
        public Truck()
        {

        }

        public Truck(string regionGeoJson, string numberPlate)
        {
            RegionGeoJson = regionGeoJson;
            NumberPlate = numberPlate;
        }

        public Truck(string regionGeoJson, string numberPlate, string hopType, string code, string description, int processingDelayMins, string locationName, double lat, double lon, int traveltimeMins)
        {
            RegionGeoJson = regionGeoJson;
            NumberPlate = numberPlate;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            Lat = lat;
            Lon = lon;
            TraveltimeMins = traveltimeMins;
        }
        public string RegionGeoJson { get; set; }

        public string NumberPlate { get; set; }
    }
}
