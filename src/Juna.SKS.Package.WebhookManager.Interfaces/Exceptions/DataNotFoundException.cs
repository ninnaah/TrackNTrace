using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.WebhookManager.Interfaces.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class DataNotFoundException : DataException
    {
        public DataNotFoundException(string repo, string method):base(repo, method)
        {

        }
    }
}
