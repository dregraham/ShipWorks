using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Warehouse.DTO.Configuration
{
    [Obfuscation]
    public class ConfigurationAddress
    {
        public string FullName { get; set; }

        public string Company { get; set; }

        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Zip { get; set; }

        public string Country { get; set; }

        public string Phone { get; set; }
    }
}
