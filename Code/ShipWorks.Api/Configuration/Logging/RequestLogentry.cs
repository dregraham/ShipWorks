using System.Reflection;
using Interapptive.Shared.Extensions;
using Microsoft.Owin;

namespace ShipWorks.Api.Configuration.Logging
{
    /// <summary>
    /// Request to be logged
    /// </summary>
    [Obfuscation(Exclude = true)]
    public struct RequestLogEntry
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
