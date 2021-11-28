using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Transferwarehouse : Hop
    {
        public Transferwarehouse()
        {

        }

        public Transferwarehouse(Geometry region, string logisticsPartner, string logisticsPartnerUrl)
        {
            Region = region;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
        }
        public Transferwarehouse(int id, Geometry region, string logisticsPartner, string logisticsPartnerUrl, string hopType, string code, 
            string description, int processingDelayMins, string locationName, GeoCoordinate locationCoordinates)
        {
            Id = id;
            Region= region;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
            HopType = hopType;
            Code = code;
            Description = description;
            ProcessingDelayMins = processingDelayMins;
            LocationName = locationName;
            LocationCoordinates = locationCoordinates;
        }
        public virtual Geometry Region { get; set; }

        public string LogisticsPartner { get; set; }

        public string LogisticsPartnerUrl { get; set; }
    }
}
