using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class RerouteOrderItems
    {
        [JsonProperty("pathParameters")]
        public PathParameters PathParameters { get; set; }

        [JsonProperty("parsedBody")]
        public ItemsToReroute ItemsToReroute { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class PathParameters
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ItemsToReroute
    {
        [JsonProperty("fromWarehouseId")]
        public string FromWarehouseId { get; set; }

        [JsonProperty("items")]
        public IEnumerable<ItemQuantity> Items { get; set; }
    }

    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public class ItemQuantity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }
    }
}
