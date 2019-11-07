using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    public class RakutenErrors
    {
        /// <summary>
        /// List of resources that caused errors
        /// </summary>
        [JsonProperty("failedResources")]
        public IList<string> FailedResources { get; set; }

        /// <summary>
        /// List of errors from the full request
        /// </summary>
        [JsonProperty("common")]
        public IList<RakutenError> Common { get; set; }

        /// <summary>
        /// List of errors for each resource that failed
        /// </summary>
        [JsonProperty("specific")]
        public Dictionary<string, IList<RakutenError>> Specific { get; set; }
    }

    public class RakutenError
    {
        /// <summary>
        /// The Rakuten API error code
        /// </summary>
        [JsonProperty("errorCode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// A short version of the error message
        /// </summary>
        [JsonProperty("shortMessage")]
        public string ShortMessage { get; set; }

        /// <summary>
        /// The complete error message
        /// </summary>
        [JsonProperty("longMessage")]
        public string LongMessage { get; set; }
    }
}
