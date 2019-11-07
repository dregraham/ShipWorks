using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShipWorks.Stores.Platforms.Rakuten.DTO
{
    public class RakutenErrors
    {
        [JsonProperty("failedResources")]
        IList<string> FailedResources { get; set; }

        [JsonProperty("common")]
        IList<RakutenError> Common { get; set; }

        [JsonProperty("specific")]
        Dictionary<string, IList<RakutenError>> Specific { get; set; }
    }

    public class RakutenError
    {
        [JsonProperty("errorCode")]
        int ErrorCode { get; set; }

        [JsonProperty("shortMessage")]
        string ShortMessage { get; set; }

        [JsonProperty("longMessage")]
        string LongMessage { get; set; }
    }
}
