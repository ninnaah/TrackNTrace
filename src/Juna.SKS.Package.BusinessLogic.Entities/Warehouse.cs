using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Entities
{
    public class Warehouse : Hop
    { 
        public int? Level { get; set; }

        public List<Hop> NextHops { get; set; }
    }
}
