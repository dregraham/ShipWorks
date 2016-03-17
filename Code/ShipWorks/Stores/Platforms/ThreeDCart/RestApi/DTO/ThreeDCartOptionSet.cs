﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.ThreeDCart.RestApi.DTO
{
    public class ThreeDCartOptionSet
    {
        [JsonProperty("OptionSetID")]
        public int OptionSetID { get; set; }

        [JsonProperty("OptionSetName")]
        public string OptionSetName { get; set; }

        [JsonProperty("OptionSorting")]
        public int OptionSorting { get; set; }

        [JsonProperty("OptionRequired")]
        public bool OptionRequired { get; set; }

        [JsonProperty("OptionType")]
        public string OptionType { get; set; }

        [JsonProperty("OptionURL")]
        public string OptionURL { get; set; }

        [JsonProperty("OptionAdditionalInformation")]
        public string OptionAdditionalInformation { get; set; }

        [JsonProperty("OptionSizeLimit")]
        public int OptionSizeLimit { get; set; }

        [JsonProperty("OptionList")]
        public IList<ThreeDCartOption> OptionList { get; set; }
    }
}