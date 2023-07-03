﻿using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoRestShipment
{
    /// <summary>
    /// Magento ShipmentItem
    /// </summary>
    public class ShipmentItem
    {
        [JsonProperty("order_item_id")]
        public long OrderItemId { get; set; }

        [JsonProperty("qty")]
        public double Qty { get; set; }
    }
}