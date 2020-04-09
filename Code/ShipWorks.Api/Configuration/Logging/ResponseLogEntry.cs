namespace ShipWorks.Api.Configuration.Logging
{
    /// <summary>
    /// ResponseLogEntry
    /// </summary>
    public struct ResponseLogEntry
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ResponseLogEntry(int statusCode, string body)
        {
            StatusCode = statusCode;
            Body = body;
        }

        /// <summary>
        /// Respons Status Code
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Response Body
        /// </summary>
        public string Body { get; }
    }
}
