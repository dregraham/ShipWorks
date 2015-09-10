using Newtonsoft.Json;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.LemonStand.DTO
{
    class LemonStandCustomer
    {

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("is_guest")]
        public string IsGuest { get; set; }

        [JsonProperty("created_by")]
        public string CreatedBy { get; set; }

        [JsonProperty("updated_by")]
        public string UpdatedBy { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }
}
