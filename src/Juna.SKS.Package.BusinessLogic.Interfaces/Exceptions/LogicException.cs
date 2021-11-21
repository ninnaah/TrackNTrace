using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    public class LogicException : ApplicationException
    {
        public LogicException(string module, string method)
        {
            BLModule = module;
            Method = method;
        }

        public LogicException(string module, string method, string message) : base(message)
        {
            BLModule = module;
            Method = method;
        }

        public LogicException(string module, string method, string message, Exception innerException) : base(message, innerException)
        {
            BLModule = module;
            Method = method;
        }

        public string BLModule { get; }
        public string Method { get; }
    }
}
