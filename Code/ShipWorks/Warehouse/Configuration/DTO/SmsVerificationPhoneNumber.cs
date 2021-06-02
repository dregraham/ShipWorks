using Newtonsoft.Json;

namespace ShipWorks.Warehouse.Configuration.DTO
{
    public class SmsVerificationPhoneNumber
    {
        [JsonProperty("smsVerifiedPhoneNumber")]
        public string SmsVerifiedPhoneNumber { get; set; }
    }
}