using System;
using System.Net;
using System.Reflection;

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
        public HealthCheckResponse(Guid instanceId)
        {
            InstanceId = instanceId;
        }

        /// <summary>
        /// The ShipWorks instance ID that is running the API
        /// </summary>
        public Guid InstanceId { get; }
    }
}
