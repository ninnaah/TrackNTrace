using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class HopArrival
    {
        public HopArrival()
        {

        }
        public HopArrival(int id, string code, string description, DateTime dateTime)
        {
            Id = id;
            Code = code;
            Description = description;
            DateTime = dateTime;
        }
        public int Id { get; set; }
        public string Code { get; set; }

        public string Description { get; set; }

        public DateTime? DateTime { get; set; }
    }
}
