using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.Http;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Controller for checking the status of the ShipWorks Api
    /// </summary>
    [ApiVersion("1.0")]
    [RoutePrefix("shipworks/api/v{version:apiVersion}/healthcheck")]
    public class HealthCheckController : ApiController
    {
        private readonly IShipWorksSession session;

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
        /// <response code="200">The API is operational. Returns the ShipWorks instance ID of the API</response>
        /// <response code="500">The API is not able to retrieve the ShipWorks instance ID</response>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            return session?.InstanceID != null ?
                Request.CreateResponse(HttpStatusCode.OK, new HealthCheckResponse(session.InstanceID)) :
                Request.CreateResponse(HttpStatusCode.InternalServerError, "Failed to load ShipWorks instance ID");
        }
    }
}
