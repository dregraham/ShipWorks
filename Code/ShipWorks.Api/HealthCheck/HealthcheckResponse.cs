using System;
using System.Reflection;
using Newtonsoft.Json;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// A simple status indicating the health of the API and the ShipWorks instance id. 
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class HealthCheckResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HealthCheckResponse(Guid instanceId, string status)
        {
            InstanceId = instanceId;
            Status = status;
        }

        /// <summary>
        /// The status of the ShipWorks API
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; }

        /// <summary>
        /// The ShipWorks instance ID that is running the API
        /// </summary>
        [JsonProperty("instanceId")]
        public Guid InstanceId { get; }
    }
}
