using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Juna.SKS.Package.Services.DTOs.Models;
using System.Diagnostics.CodeAnalysis;

namespace Juna.SKS.Package.Services.DTOs.Converter
{
    [ExcludeFromCodeCoverage]
    public class HopJsonConverter : JsonCreationConverter<Hop>
    {
        protected override Hop Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException("jObject");

            if (jObject["numberPlate"] != null)
            {
                return new Truck();
            }
            else if (jObject["level"] != null)
            {
                return new Warehouse();
            }
            else
            {
                return new TransferWarehouse();
            }
        }
    }
}
