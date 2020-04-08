using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using log4net;
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
        private readonly IApiLogEntry shipworksApiLog;
        private readonly ILog middlewareLogger;

        /// <summary>
        /// Constructor
        /// </summary>
        public LoggingMiddleware(OwinMiddleware next)
            : base(next)
        {
            shipworksApiLog = new LogEntryFactory().GetLogEntry(ApiLogSource.ShipWorksAPI, "API", LogActionType.Other);
            middlewareLogger = LogManager.GetLogger("ApiMiddleware");
        }

        /// <summary>
        /// Log requests and responses
        /// </summary>
        public override async Task Invoke(IOwinContext context)
        {
            DateTime now = DateTime.Now;

            if (ShouldLog(context))
            {
                shipworksApiLog.LogRequest(BuildRequestLog(context.Request), "json");
            }

            await Next.Invoke(context);

            if (ShouldLog(context))
            {
                shipworksApiLog.LogResponse(BuildResponseLog(context.Response), "json");
            }

            LogContext(context, now);
        }

        private void LogContext(IOwinContext context, DateTime now)
        {
            List<string> dataToLog = new List<string>
            {
                context.Request.RemoteIpAddress,
                context.Authentication?.User?.Identity.Name,
                now.ToString("MM/dd/yyyy, h:mm:tt"),
                "ShopWorks.API",
                context.Request.Uri.Host,
                context.Request.LocalIpAddress,
                (now-DateTime.Now).Duration().TotalMilliseconds.ToString(),
                context.Response.ContentLength?.ToString(),
                context.Response.StatusCode.ToString(),
                "-", // Windows status code
                context.Request.Method,
                context.Request.Uri.LocalPath,
                "-" // Parameters
            };
            string toLog = string.Join(", ", dataToLog.Select(d=>string.IsNullOrWhiteSpace(d) ? "-" : d));
            middlewareLogger.Info(toLog);
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
