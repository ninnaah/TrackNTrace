using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Interfaces.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DataException : ApplicationException
    {
        public DataException(string repo, string method)
        {
            Repository = repo;
            Method = method;
        }

        public DataException(string repo, string method, string message) : base(message)
        {
            Repository = repo;
            Method = method;
        }

        public DataException(string repo, string method, string message, Exception innerException):base(message, innerException)
        {
            Repository = repo;
            Method = method;
        }

        public string Repository { get; }
        public string Method { get; }
    }
}
