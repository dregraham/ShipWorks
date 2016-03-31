using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    [Obfuscation(Feature = "PreserveLiteralValues", Exclude = true, StripAfterObfuscation = false)] 
    public class ThreeDCartQuestion
    {
        [JsonProperty("OrderID")]
        public int OrderID { get; set; }

        [JsonProperty("QuestionID")]
        public int QuestionID { get; set; }

        [JsonProperty("QuestionTitle")]
        public string QuestionTitle { get; set; }

        [JsonProperty("QuestionAnswer")]
        public string QuestionAnswer { get; set; }

        [JsonProperty("QuestionType")]
        public string QuestionType { get; set; }

        [JsonProperty("QuestionCheckoutStep")]
        public int QuestionCheckoutStep { get; set; }

        [JsonProperty("QuestionSorting")]
        public int QuestionSorting { get; set; }

        [JsonProperty("QuestionDiscountGroup")]
        public int QuestionDiscountGroup { get; set; }
    }
}