using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice
{
    public class MagentoInvoiceItem
    {
        [JsonProperty("order_item_id")]
        public long MagentoOrderItemId { get; set; }

        [JsonProperty("qty")]
        public double Qty { get; set; }
    }
}