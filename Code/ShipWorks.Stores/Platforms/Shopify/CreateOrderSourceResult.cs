using System.Reflection;

namespace ShipWorks.Stores.Platforms.Shopify
{
    [Obfuscation(Exclude = true)]
    public class CreateOrderSourceResult
    {
        public string StoreType { get; set; }
        public string Domain { get; set; }
        public string OrderSourceId { get; set; }
    }
}