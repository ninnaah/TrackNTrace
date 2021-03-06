using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juna.SKS.Package.DataAccess.Entities
{
    [ExcludeFromCodeCoverage]
    public class Recipient
    {
        public Recipient()
        {

        }
        public Recipient(int id, string name, string street, string postalCode, string city, string country)
        {
            Id = id;
            Name = name;
            Street = street;
            PostalCode = postalCode;
            City = city;
            Country = country;
        }
        public int Id { get; set; }
        public string Name { get; set; }

        public string Street { get; set; }

        public string PostalCode { get; set; }

        public string City { get; set; }

        public string Country { get; set; }
    }
}
