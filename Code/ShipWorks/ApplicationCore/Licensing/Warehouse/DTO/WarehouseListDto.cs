using System.Collections.Generic;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    public class WarehouseListDto
    {
        public List<Warehouse> warehouses { get; set; }
        public int count { get; set; }
        public int scannedCount { get; set; }
    }

    public class Warehouse
    {
        public string id { get; set; }
        public Details details { get; set; }
    }

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
