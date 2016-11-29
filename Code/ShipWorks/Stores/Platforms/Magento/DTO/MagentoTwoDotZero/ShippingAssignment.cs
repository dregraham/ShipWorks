﻿using System.Collections.Generic;
using Newtonsoft.Json;
using ShipWorks.Stores.Platforms.Magento.DTO.Interfaces;

namespace ShipWorks.Stores.Platforms.Magento.DTO.MagentoTwoDotZero
{
    public class ShippingAssignment
    {
        [JsonProperty("shipping")]
        public IShipping Shipping { get; set; }

        [JsonProperty("items")]
        public IList<IItem> Items { get; set; }
    }
}