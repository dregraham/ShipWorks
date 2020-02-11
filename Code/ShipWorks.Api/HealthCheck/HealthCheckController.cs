using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.Http;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Check the status of the ShipWorks Api
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("shipworks/api/v{version:apiVersion}/healthcheck")]
    public class HealthCheckController : ApiController
    {
        private readonly IShipWorksSession session;
        private const HttpStatusCode ok = HttpStatusCode.OK;

        /// <summary>
        /// Constructor
        /// </summary>
        public HealthCheckController(IShipWorksSession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Check the status of the ShipWorks Api
        /// </summary>
        /// <response code="200">The API is operational</response>
        /// <response code="500">The API is not able to retrieve the ShipWorks instance ID</response>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            return session?.InstanceID != null ?
                Request.CreateResponse(ok, new HealthCheckResponse(ok, session.InstanceID)) :
                Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to load ShipWorks instance ID");
        }
    }
}
