using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    public class RakutenCampaign
    {
        [JsonProperty("campaignName")]
        public string CampaignName { get; set; }

        [JsonProperty("campaignInfo")]
        public RakutenCampaignInfo CampaignInfo { get; set; }
    }

    public class RakutenCampaignInfo
    {
        [JsonProperty("discountType")]
        public string DiscountType { get; set; }

        [JsonProperty("discountValue")]
        public double DiscountValue { get; set; }
    }
}
