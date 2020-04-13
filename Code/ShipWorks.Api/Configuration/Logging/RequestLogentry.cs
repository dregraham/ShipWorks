using System.Reflection;

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
        public RequestLogEntry(string path, string body)
        {
            Path = path;
            Body = body;
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
