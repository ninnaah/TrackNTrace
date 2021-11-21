using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Interfaces.Exceptions
{
    public class DataNotFoundException : DataException
    {
        public DataNotFoundException(string repo, string method):base(repo, method)
        {

        }
    }
}
