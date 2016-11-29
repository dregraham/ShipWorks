using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotOne
{
    public class Payment : IPayment
    {
        [JsonProperty("account_status")]
        public string AccountStatus { get; set; }

        [JsonProperty("additional_information")]
        public IList<string> AdditionalInformation { get; set; }

        [JsonProperty("amount_ordered")]
        public double AmountOrdered { get; set; }

        [JsonProperty("base_amount_ordered")]
        public double BaseAmountOrdered { get; set; }

        [JsonProperty("base_shipping_amount")]
        public double BaseShippingAmount { get; set; }

        [JsonProperty("cc_exp_year")]
        public string CcExpYear { get; set; }

        [JsonProperty("cc_last4")]
        public string CcLast4 { get; set; }

        [JsonProperty("cc_ss_start_month")]
        public string CcSsStartMonth { get; set; }

        [JsonProperty("cc_ss_start_year")]
        public string CcSsStartYear { get; set; }

        [JsonProperty("entity_id")]
        public int EntityId { get; set; }

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("parent_id")]
        public int ParentId { get; set; }

        [JsonProperty("shipping_amount")]
        public double ShippingAmount { get; set; }
    }
}