using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class HopArrival
    {
        public HopArrival()
        {

        }
        public HopArrival(string code, string description, DateTime dateTime)
        {
            Code = code;
            Description = description;
            DateTime = dateTime;
        }
        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
