using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.Configuration.Stores.DTO
{
    [Obfuscation]
    public class Address
    {
        public string City { get; set; }
        public string StateProvCode { get; set; }
        public string CountryCode { get; set; }
        public string PostalCode { get; set; }
        public string Company { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
    }
}
