using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true)]
    public class ThreeDCartProduct
    {
        public string WarehouseBin { get; set; }

        public string MainImageFile { get; set; }

        public string ThumbnailFile { get; set; }

        [JsonProperty("SKUInfo")]
        public ThreeDCartSkuInfo SkuInfo { get; set; }
    }
}