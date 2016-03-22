using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartProduct
    {
        [JsonProperty("WarehouseBin")]
        public string WarehouseBin { get; set; }

        [JsonProperty("MainImageFile")]
        public string MainImageFile { get; set; }

        [JsonProperty("ThumbnailFile")]
        public string ThumbnailFile { get; set; }
    }
}