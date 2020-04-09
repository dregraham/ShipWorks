using System;
using System.Collections.Generic;
using System.IO;
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

            bool shouldLog = ShouldLog(context);

            if (shouldLog)
            {
                shipworksApiLog.LogRequest(BuildRequestLog(context.Request), "json");
            }

            // Convert write only context stream to readable memory stream
            Stream responseBodyStream = context.Response.Body;
            MemoryStream responseBuffer = new MemoryStream();
            context.Response.Body = responseBuffer;

            // Actually run request
            await Next.Invoke(context);

            // Get the response body
            responseBuffer.Seek(0, SeekOrigin.Begin);
            var responseBody = new StreamReader(responseBuffer).ReadToEnd();

            // reset the stream
            responseBuffer.Seek(0, SeekOrigin.Begin);
            await responseBuffer.CopyToAsync(responseBodyStream);

            if (shouldLog)
            {
                shipworksApiLog.LogResponse(BuildResponseLog(context.Response.StatusCode, responseBody), "json");
            }

            LogContext(context, now);
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
        private string BuildResponseLog(int statusCode, string body)
        {
            return JsonConvert.SerializeObject(new ResponseLogEntry(statusCode, body));
        }

        /// <summary>
        /// Logs the request and response in IISLog format
        /// </summary>
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
            string toLog = string.Join(", ", dataToLog.Select(d => string.IsNullOrWhiteSpace(d) ? "-" : d));
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
        /// ResponseLogEntry
        /// </summary>
        public class ResponseLogEntry
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public ResponseLogEntry(int statusCode, string body)
            {
                StatusCode = statusCode;
                Body = body;
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
