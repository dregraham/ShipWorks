using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Warehouse.Configuration.DTO
{
    [Obfuscation]
    public class SmsVerificationPhoneNumber
    {
        [JsonProperty("smsVerifiedPhoneNumber")]
        public string SmsVerifiedPhoneNumber { get; set; }
    }
}