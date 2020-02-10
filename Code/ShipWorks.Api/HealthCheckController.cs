using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Web.Http;
using ShipWorks.ApplicationCore;

namespace ShipWorks.Api
{
    /// <summary>
    /// Check the status of the ShipWorks Api
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
        /// <response code="200">The API is operational</response>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Get()
        {
            string responseBody = "OK" +
                                  $"{Environment.NewLine}" +
                                  $"{Environment.NewLine}" +
                                  $"ShipWorks Instance ID: {session.InstanceID:D}";

            return Request.CreateResponse(HttpStatusCode.OK, responseBody);
        }
    }
}
