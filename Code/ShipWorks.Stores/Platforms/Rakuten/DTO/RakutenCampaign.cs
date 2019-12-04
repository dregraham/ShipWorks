using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    [Obfuscation(Exclude = true)]
    public class RakutenCampaign
    {
        [JsonProperty("campaignName")]
        public string CampaignName { get; set; }

        [JsonProperty("campaignInfo")]
        public RakutenCampaignInfo CampaignInfo { get; set; }
    }

    [Obfuscation(Exclude = true)]
    public class RakutenCampaignInfo
    {
        [JsonProperty("discountType")]
        public string DiscountType { get; set; }

        [JsonProperty("discountValue")]
        public double DiscountValue { get; set; }
    }
}
