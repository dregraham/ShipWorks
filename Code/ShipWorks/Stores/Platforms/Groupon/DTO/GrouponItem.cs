using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Groupon.DTO
{
    /// <summary>
    /// Order dto object that gets populated by the JsonConvert.DeserializeObject 
    /// </summary>
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


        public string sku { get; set; }
        public string status { get; set; }
        public string permalink { get; set; }
        public string name { get; set; }
        public double weight { get; set; }
        public string gg_account_number { get; set; }
        public string po_number { get; set; }
        public string channel_sku_provided { get; set; }
        public string fulfillment_lineitem_id { get; set; }
        public decimal unit_price { get; set; }
        public string bom_sku { get; set; }
        public string kitting_details { get; set; }
        public string ci_lineitemid { get; set; }
        public string gift_message { get; set; }
        public int quantity { get; set; }



    }
}
