using System.Collections.Generic;
using System.Reflection;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class WarehouseListDto
    {
        public List<Warehouse> warehouses { get; set; } = new List<Warehouse>();
        public int count { get; set; }
        public int scannedCount { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Warehouse
    {
        public string id { get; set; }
        public Details details { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class Details
    {
        public string zip { get; set; }
        public string code { get; set; }
        public string city { get; set; }
        public string street { get; set; }
        public int customerID { get; set; }
        public string name { get; set; }
        public string id { get; set; }
        public string state { get; set; }
        public string shipWorksLink { get; set; }
    }
}
