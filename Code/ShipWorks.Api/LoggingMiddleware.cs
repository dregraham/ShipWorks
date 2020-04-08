using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Microsoft.Owin;
using Newtonsoft.Json;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Api
{
    /// <summary>
    /// Middleware for handling logging for the ShipWorks API
    /// </summary>
    public class LoggingMiddleware : OwinMiddleware
    {
        IApiLogEntry logEntry;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoggingMiddleware(OwinMiddleware next)
            : base(next)
        {
            logEntry = new LogEntryFactory().GetLogEntry(ApiLogSource.ShipWorksAPI, "API", LogActionType.Other);
        }

        /// <summary>
        /// Log requests and responses
        /// </summary>
        public override async Task Invoke(IOwinContext context)
        {
            if (ShouldLog(context))
            {
                logEntry.LogRequest(BuildRequestLog(context.Request), "json");
            }

            await Next.Invoke(context);

            if (ShouldLog(context))
            {
                logEntry.LogResponse(BuildResponseLog(context.Response), "json");
            }
        }

        /// <summary>
        /// Check to see if the given context should be logged
        /// </summary>
        /// <remarks>some endpoints like healthcheck get hit too often to log</remarks>
        /// <returns>bool</returns>
        private bool ShouldLog(IOwinContext context)
        {
            string path = context.Request.Path.ToString();

            return !path.Contains("healthcheck") &&
                !path.Contains("swagger");
        }

        /// <summary>
        /// Build a string containing the request that gets logged
        /// </summary>
        string BuildRequestLog(IOwinRequest request)
        {
            return JsonConvert.SerializeObject(new RequestLogEntry(request));
        }


        /// <summary>
        /// Build a string containing the response that gets logged
        /// </summary>
        string BuildResponseLog(IOwinResponse response)
        {
            return JsonConvert.SerializeObject(new ResponseLogEntry(response));
        }

        public class ResponseLogEntry
        {
            public ResponseLogEntry(IOwinResponse response)
            {
                StatusCode = response.StatusCode;
                Body = response.Body.ConvertToString();
            }

            public int StatusCode { get; }
            public string Body { get; }
        }

        /// <summary>
        /// Request to be logged
        /// </summary>
        public class RequestLogEntry
        {
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="request"></param>
            public RequestLogEntry(IOwinRequest request)
            {
                Path = request.Path.ToString();
                Body = request.Body.ConvertToString();
            }

            /// <summary>
            /// The Path
            /// </summary>
            public string Path { get; }

            /// <summary>
            /// The Body
            /// </summary>
            public string Body { get; }
        }
    }
}
