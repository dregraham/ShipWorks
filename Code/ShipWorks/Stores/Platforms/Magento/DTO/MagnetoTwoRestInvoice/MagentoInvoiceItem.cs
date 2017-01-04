using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice
{
    public class MagentoInvoiceItem
    {
        [JsonProperty("orderItemId")]
        public long MagentoOrderItemId { get; set; }

        [JsonProperty("qty")]
        public double Qty { get; set; }
    }
}