using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestInvoice
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
}