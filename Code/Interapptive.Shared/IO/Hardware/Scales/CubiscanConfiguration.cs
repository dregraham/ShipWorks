namespace Interapptive.Shared.IO.Hardware.Scales
{
    /// <summary>
    /// Cubiscan Configuration
    /// </summary>
    public class CubiscanConfiguration
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CubiscanConfiguration(bool isConfigured, string ipAddress, int port)
        {
            IsConfigured = isConfigured;
            IpAddress = ipAddress;
            Port = port;
        }

        /// <summary>
        /// Is a cubiscan configured for this computer
        /// </summary>
        public bool IsConfigured { get; }

        /// <summary>
        /// IP Address of the Cubiscan
        /// </summary>
        public string IpAddress { get; }

        /// <summary>
        /// Port of the Cubiscan
        /// </summary>
        public int Port { get; }
    }
}
