using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the JsonConvert.DeserializeObject 
    /// </summary>
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)]
    public class GrouponItem
    {
        //"line_items": [{
        //    "sku": "888849001232",
        //    "status": "open",
        //    "permalink": "gg-quest-protein-bars-1",
        //    "name": "12-Count Quest Nutrition Protein Bars: S'mores",
        //    "weight": "1.7",
        //    "gg_account_number": "896098",
        //    "po_number": "006C000000wDZmf-11561881",
        //    "channel_sku_provided": "a0YC000000bC8WI",
        //    "fulfillment_lineitem_id": "79862167",
        //    "unit_price": 24.99,
        //    "bom_sku": "",
        //    "opp_name": "",
        //    "kitting_details": "",
        //    "ci_lineitemid": 146599167,
        //    "gift_message": "",
        //    "quantity": 1
        //}]

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("permalink")]
        public string Permalink { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("weight")]
        public string Weight { get; set; }

        [JsonProperty("gg_account_number")]
        public string GrouponAccountNumber { get; set; }

        [JsonProperty("po_number")]
        public string PoNumber { get; set; }

        [JsonProperty("channel_sku_provided")]
        public string ChannelSkuProvided { get; set; }

        [JsonProperty("fulfillment_lineitem_id")]
        public string FulfillmentLineitemId { get; set; }

        [JsonProperty("unit_price")]
        public string UnitPrice { get; set; }

        [JsonProperty("bom_sku")]
        public string BomSku { get; set; }

        [JsonProperty("kitting_details")]
        public string KittingDetails { get; set; }

        [JsonProperty("ci_lineitemid")]
        public string GrouponLineitemId { get; set; }

        [JsonProperty("gift_message")]
        public string GiftMessage { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }
    }
}
