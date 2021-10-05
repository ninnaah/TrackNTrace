using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Transferwarehouse : Hop
    {
        public string RegionGeoJson { get; set; }

        public string LogisticsPartner { get; set; }

        public string LogisticsPartnerUrl { get; set; }
    }
}
