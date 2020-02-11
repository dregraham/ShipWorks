using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
