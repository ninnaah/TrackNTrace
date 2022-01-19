using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Juna.SKS.Package.Website.Models
{
    public class Parcel
    {
        public float? Weight { get; set; }

        public Recipient Recipient { get; set; }

        public Recipient Sender { get; set; }
    }
}