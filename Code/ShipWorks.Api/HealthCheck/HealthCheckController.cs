using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Microsoft.Web.Http;
using ShipWorks.ApplicationCore;
using Swashbuckle.Swagger.Annotations;

namespace ShipWorks.Api.HealthCheck
{
    /// <summary>
    /// Controller for checking the status of the ShipWorks API
    /// </summary>
    
    [ApiVersion("1.0")]
    [RoutePrefix("shipworks/api/v{version:apiVersion}/healthcheck")]
    [Obfuscation(Exclude = true)]
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
        /// Returns a simple status indicating the health of the API and the ShipWorks instance id. 
        /// The instanceId is a GUID that represents that instance of ShipWorks. 
        /// If there are multiple instances of ShipWorks installed, the instanceId can be used to 
        /// determine which instance of ShipWorks is serving up this API.
        /// </summary>
        [HttpGet]
        [Route("")]
        [SwaggerResponse(HttpStatusCode.OK, 
            Type=typeof(HealthCheckResponse), 
            Description = "The service is functional")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "The service is not functional")]
        public HttpResponseMessage Get()
        {
            try
            {
                return CreateResponse(HttpStatusCode.OK, "OK");
            }
            catch (Exception ex)
            {
                return CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Generate an HttpResponseMessage using the given status code and message
        /// </summary>
        private HttpResponseMessage CreateResponse(HttpStatusCode statusCode, string statusMessage) =>
            Request.CreateResponse(statusCode, new HealthCheckResponse(session.InstanceID, statusMessage));
    }
}
