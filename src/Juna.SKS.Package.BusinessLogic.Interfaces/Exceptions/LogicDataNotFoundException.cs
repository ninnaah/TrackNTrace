using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class LogicDataNotFoundException : LogicException
    {
        public LogicDataNotFoundException(string module, string method, string message) : base(module, method, message)
        {

        }
    }
}
