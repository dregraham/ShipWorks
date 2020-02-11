using System;
using System.Net;
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
        public HealthCheckResponse(HttpStatusCode status, Guid instanceId)
        {
            Status = status;
            InstanceId = instanceId;
        }

        /// <summary>
        /// The status code of the response
        /// </summary>
        public HttpStatusCode Status { get; }

        /// <summary>
        /// The ShipWorks instance ID that is running the API
        /// </summary>
        public Guid InstanceId { get; }
    }
}
