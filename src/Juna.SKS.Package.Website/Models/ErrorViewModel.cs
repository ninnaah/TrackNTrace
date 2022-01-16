using System;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.Website.Models
{
    [ExcludeFromCodeCoverage]
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
