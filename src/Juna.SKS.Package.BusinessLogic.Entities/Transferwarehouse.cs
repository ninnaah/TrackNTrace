﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Transferwarehouse : Hop
    {
        public Transferwarehouse()
        {

        }

        public Transferwarehouse(string regionGeoJson, string logisticsPartner, string logisticsPartnerUrl)
        {
            RegionGeoJson = regionGeoJson;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
        }
        public Transferwarehouse(string regionGeoJson, string logisticsPartner, string logisticsPartnerUrl, string hopType, string code, 
            string description, int processingDelayMins, string locationName, double lat, double lon, int traveltimeMins)
        {
            RegionGeoJson = regionGeoJson;
            LogisticsPartner = logisticsPartner;
            LogisticsPartnerUrl = logisticsPartnerUrl;
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

        public string LogisticsPartner { get; set; }

        public string LogisticsPartnerUrl { get; set; }
    }
}
