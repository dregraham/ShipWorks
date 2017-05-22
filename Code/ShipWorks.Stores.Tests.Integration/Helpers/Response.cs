using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using ShipWorks.Tests.Shared;

namespace ShipWorks.Stores.Tests.Integration.Helpers
{
    /// <summary>
    /// Create responses for use with RestSharp requests
    /// </summary>
    public static class Response
    {
        /// <summary>
        /// Create a success result with the given content
        /// </summary>
        public static IRestResponse Success(string content) =>
            new RestResponse { StatusCode = HttpStatusCode.OK, Content = content, ContentLength = content.Length };

        /// <summary>
        /// Create a NoConent response
        /// </summary>
        public static IRestResponse NoContent() =>
            new RestResponse { StatusCode = HttpStatusCode.NoContent };

        /// <summary>
        /// Create a 404 NotFound response
        /// </summary>
        /// <returns></returns>
        internal static IRestResponse NotFound()
        {
            string message = "[{\"status\":404,\"message\":\"The requested resource was not found.\"}]";
            return new RestResponse { StatusCode = HttpStatusCode.NotFound, Content = message, ContentLength = message.Length };
        }

        /// <summary>
        /// Create a success response with the contents from the given embedded resource
        /// </summary>
        internal static IRestResponse SuccessFromResource(string path) =>
            SuccessFromResource(path, x => x);

        /// <summary>
        /// Create a success response with the contents from the given embedded resource
        /// </summary>
        internal static IRestResponse SuccessFromResource(string path, Func<string, string> alterResponse)
        {
            string response = CallingAssembly().GetEmbeddedResourceString(path);
            return Success(alterResponse(response));
        }

        /// <summary>
        /// Get the assembly of the calling method
        /// </summary>
        public static Assembly CallingAssembly()
        {
            StackTrace stackTrace = new System.Diagnostics.StackTrace();
            var assembly = stackTrace.GetFrames()
                .Select(x => x.GetMethod().DeclaringType)
                .Where(x => x != typeof(Response))
                .Select(x => x.Assembly)
                .FirstOrDefault();

            if (assembly == null)
            {
                throw new InvalidOperationException("Could not get calling assembly");
            }

            return assembly;
        }
    }
}
