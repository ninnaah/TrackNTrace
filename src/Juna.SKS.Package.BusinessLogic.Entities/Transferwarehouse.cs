using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    [ExcludeFromCodeCoverage]
    public class Transferwarehouse : Hop
    {
        public Transferwarehouse()
        {

        }

        public Transferwarehouse(string regionGeoJson
            , string logisticsPartner, string logisticsPartnerUrl)
        {
            RegionGeoJson = regionGeoJson;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
        }
        public Transferwarehouse(string regionGeoJson, string logisticsPartner, string logisticsPartnerUrl, string hopType, string code, 
            string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            RegionGeoJson = regionGeoJson;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public virtual string RegionGeoJson { get; set; }

        public string LogisticsPartner { get; set; }

        public string LogisticsPartnerUrl { get; set; }
    }
}
