using System.Linq;
using System.Net;
using log4net;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Utility class to get
    /// </summary>
    public class NetworkUtility : INetworkUtility
    {
        private readonly ILog log;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkUtility"/> class.
        /// </summary>
        public  NetworkUtility():this(LogManager.GetLogger(typeof(NetworkUtility)))
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkUtility"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public NetworkUtility(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <returns>
        /// A version 4 IP address.
        /// </returns>
        /// <exception cref="Interapptive.Shared.Net.NetworkException">ShipWorks could not obtain the IP address of this machine.</exception>
        public string GetIPAddress()
        {
            string ipAddress;

            // Grab the version 4 IP address of the client machine before submitting the registration to Stamps.com
            string hostName = Dns.GetHostName();
            if (!string.IsNullOrEmpty(hostName))
            {
                ipAddress = Dns.GetHostAddresses(hostName).First(ip => !IPAddress.IsLoopback(ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                log.InfoFormat("IP address for retrieved for this machine was {0}", ipAddress);
            }
            else
            {
                throw new NetworkException("ShipWorks could not obtain the IP address of this machine.");
            }

            return ipAddress;
        }
    }
}