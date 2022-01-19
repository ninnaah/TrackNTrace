using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.BusinessLogic.Interfaces.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ValidatorException : LogicException
    {
         public ValidatorException(string module, string method, string message) : base(module, method, message)
         {

         }

    }
}
