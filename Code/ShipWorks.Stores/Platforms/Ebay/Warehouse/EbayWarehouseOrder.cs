using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Ebay.Warehouse
{
    /// <summary>
    /// Ebay Warehouse Order
    /// </summary>
    [Obfuscation]
    public class EbayWarehouseOrder
    {
        /// <summary>
        /// The EbayBuyerID of the EbayOrder
        /// </summary>
        [JsonProperty("ebayBuyerId")]
        public string EbayBuyerID { get; set; }

        /// <summary>
        /// The SelectedShippingMethod of the EbayOrder
        /// </summary>
        public int SelectedShippingMethod { get; set; }

        /// <summary>
        /// The SellingManagerRecord of the EbayOrder
        /// </summary>
        public int SellingManagerRecord { get; set; }

        /// <summary>
        /// The GspEligible of the EbayOrder
        /// </summary>
        public bool GspEligible { get; set; }

        /// <summary>
        /// The GspFirstName of the EbayOrder
        /// </summary>
        public string GspFirstName { get; set; }

        /// <summary>
        /// The GspLastName of the EbayOrder
        /// </summary>
        public string GspLastName { get; set; }

        /// <summary>
        /// The GspStreet1 of the EbayOrder
        /// </summary>
        public string GspStreet1 { get; set; }

        /// <summary>
        /// The GspStreet2 of the EbayOrder
        /// </summary>
        public string GspStreet2 { get; set; }

        /// <summary>
        /// The GspCity of the EbayOrder
        /// </summary>
        public string GspCity { get; set; }

        /// <summary>
        /// The GspStateProvince of the EbayOrder
        /// </summary>
        public string GspStateProvince { get; set; }

        /// <summary>
        /// The GspPostalCode of the EbayOrder
        /// </summary>
        public string GspPostalCode { get; set; }

        /// <summary>
        /// The GspCountryCode of the EbayOrder
        /// </summary>
        public string GspCountryCode { get; set; }

        /// <summary>
        /// The GspReferenceID of the EbayOrder
        /// </summary>
        [JsonProperty("gspReferenceId")]
        public string GspReferenceID { get; set; }

        /// <summary>
        /// The GuaranteedDelivery of the EbayOrder
        /// </summary>
        public bool GuaranteedDelivery { get; set; }
    }
}
