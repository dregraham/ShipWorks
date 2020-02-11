using System;
using System.Net;
using System.Reflection;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Response DTO for healtheck
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
        /// Status
        /// </summary>
        public HttpStatusCode Status { get; }
        
        /// <summary>
        /// InstanceID
        /// </summary>
        public Guid InstanceId { get; }
    }
}
