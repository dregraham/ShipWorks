using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Warehouse.Orders.DTO
{
    [Obfuscation]
    public class WarehouseOrderItem
    {
        public WarehouseOrderItem()
        {
            ItemAttributes = new List<WarehouseOrderItemAttribute>();
        }

        public string StoreOrderItemID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SKU { get; set; }
        public string ISBN { get; set; }
        public string UPC { get; set; }
        public string HarmonizedCode { get; set; }
        public string Brand { get; set; }
        public string MPN { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Thumbnail { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitCost { get; set; }
        public double Quantity { get; set; }
        public double Weight { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string LocalStatus { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        public string Custom1 { get; set; }

        public string Custom2 { get; set; }

        public string Custom3 { get; set; }

        public string Custom4 { get; set; }

        public string Custom5 { get; set; }

        public string Custom6 { get; set; }

        public string Custom7 { get; set; }

        public string Custom8 { get; set; }

        public string Custom9 { get; set; }

        public string Custom10 { get; set; }

        public IEnumerable<WarehouseOrderItemAttribute> ItemAttributes { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }
    }
}
