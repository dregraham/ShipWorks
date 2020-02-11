using System;
using System.Reflection;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// The health check response DTO
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
