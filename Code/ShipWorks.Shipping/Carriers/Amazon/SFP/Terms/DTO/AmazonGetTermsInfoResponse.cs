using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP.Terms.DTO
{
    [Obfuscation(ApplyToMembers = true, Exclude = true, StripAfterObfuscation = false)]
    public class AmazonGetTermsInfoResponse
    {
        [JsonProperty("amazonTermsVersion")]
        public string AmazonTermsVersion { get; set; }

        [JsonProperty("amazonTermsAvailableDate")]
        public string AmazonTermsAvailableDate { get; set; }

        [JsonProperty("amazonTermsDeadlineDate")]
        public string AmazonTermsDeadlineDate { get; set; }

        [JsonProperty("amazonTermsUrl")]
        public string AmazonTermsUrl { get; set; }
    }
}
