using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice
{
    public class Invoice
    {
        public Invoice()
        {
            Items = new List<MagentoInvoiceItem>();
        }

        [JsonProperty("orderId")]
        public long MagentoOrderID { get; set; }

        [JsonProperty("totalQty")]
        public double TotalQty { get; set; }

        [JsonProperty("items")]
        public IList<MagentoInvoiceItem> Items { get; set; }
    }
}