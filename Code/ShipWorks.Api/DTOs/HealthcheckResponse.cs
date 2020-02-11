using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Api.DTOs
{
    /// <summary>
    /// Response DTO for healtheck
    /// </summary>
    [Obfuscation(Exclude = true)]
    public class HealthcheckResponse
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public HealthcheckResponse(HttpStatusCode status, Guid instanceId)
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
