using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShipWorks.ApplicationCore.Licensing.Warehouse.DTO
{
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public abstract class ShipEngineStore: Store
    {
        /// <summary>
        /// The platform order source id 
        /// </summary>
        [JsonProperty("orderSourceId")]
        public string OrderSourceID { get; set; }

        /// <summary>
        /// The platform account id (Account Service AccountId)
        /// </summary>
        public string PlatformAccountId { get; set; }

        /// <summary>
        /// The platform continuation token
        /// </summary>
        [JsonProperty("continuationToken")]
        public string ContinuationToken { get; set; }
    }
}
