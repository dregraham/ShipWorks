namespace ShipWorks.Api.Configuration
{
    /// <summary>
    /// Settings for the ShipWorks API
    /// </summary>
    public class ApiSettings
    {
        /// <summary>
        /// Whether or not the API is enabled
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// The port number used by the API
        /// </summary>
        public long Port { get; set; } = 8081;
    }
}
