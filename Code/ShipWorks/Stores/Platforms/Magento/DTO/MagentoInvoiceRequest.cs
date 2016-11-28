using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO
{
    public class MagentoInvoiceRequest
    {
        public MagentoInvoiceRequest()
        {
            Invoice = new Invoice();
        }

        [JsonProperty("entity")]
        public Invoice Invoice { get; set; }
    }

    public class MagentoInvoiceItem
    {
        [JsonProperty("orderItemId")]
        public long MagentoOrderItemId { get; set; }

        [JsonProperty("qty")]
        public double Qty { get; set; }
    }

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