using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    public class RakutenOptionalCheckoutInfo
    {
        /// <summary>
        /// The name of the grouped optional checkout information
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The description of the grouped optional checkout information
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The message heading shown at checkout
        /// </summary>
        [JsonProperty("messageHeading")]
        public string MessageHeading { get; set; }

        /// <summary>
        /// The information entered or selected by the shopper during checkout
        /// </summary>
        [JsonProperty("filledInfo")]
        public IList<RakutenFilledInfo> FilledInfo { get; set; }
    }

    public class RakutenFilledInfo
    {
        /// <summary>
        /// The information title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The information description
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// The label of the UI element the information is from
        /// </summary>
        [JsonProperty("informationToFill")]
        public string InformationToFill { get; set; }

        /// <summary>
        /// The information entered or selected by the shopper during checkout
        /// </summary>
        [JsonProperty("inputValue")]
        public string InputValue { get; set; }
    }
}
