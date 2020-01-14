using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    [Obfuscation(Exclude = true)]
    public class RakutenOptionalCheckoutInfo
    {
        /// <summary>
        /// The name of the grouped optional checkout information
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The information entered or selected by the shopper during checkout
        /// </summary>
        [JsonProperty("filledInfo")]
        public List<RakutenFilledInfo> FilledInfo { get; set; }
    }

    [Obfuscation(Exclude = true)]
    public class RakutenFilledInfo
    {
        /// <summary>
        /// The information title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The information entered or selected by the shopper during checkout
        /// </summary>
        [JsonProperty("inputValue")]
        public string InputValue { get; set; }
    }
}
