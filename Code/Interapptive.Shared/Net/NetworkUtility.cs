using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public NetworkUtility()
            : this(LogManager.GetLogger(typeof(NetworkUtility)))
        { }

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

            // Grab the version 4 IP address of the client machine before submitting the registration to USPS
            string hostName = Dns.GetHostName();
            if (!string.IsNullOrEmpty(hostName))
            {
                ipAddress = Dns.GetHostAddresses(hostName).First(ip => !IPAddress.IsLoopback(ip) && ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
                log.InfoFormat("IP address retrieved for this machine was {0}", ipAddress);
            }
            else
            {
                throw new NetworkException("ShipWorks could not obtain the IP address of this machine.");
            }

            return ipAddress;
        }

        /// <summary>
        /// Gets the public ip address of this computer from various sources and uses the first valid response
        /// </summary>
        /// <returns>The public IP for this computer, or "Unknown" if it could not be retrieved</returns>
        public string GetPublicIPAddress()
        {
            return GetPublicIPAddress(true);
        }

        /// <summary>
        /// Gets the public ip address of this computer from various sources and uses the first valid response
        /// </summary>
        /// <param name="bypassProxy">Defines whether the proxy check should be skipped.
        /// If it is skipped, it will be run again without skipping the proxy check.</param>
        /// <returns>The public IP for this computer, or "Unknown" if it could not be retrieved</returns>
        public string GetPublicIPAddress(bool bypassProxy)
        {
            // Check three different services in case one (or more) of them takes too long
            List<Task<string>> tasks = new List<Task<string>>
                {
                    Task<string>.Factory.StartNew(() => GetIPFromUrl("http://checkip.dyndns.org", bypassProxy, @"Current IP Address: (\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})")),
                    Task<string>.Factory.StartNew(() => GetIPFromUrl("http://ipecho.net/plain", bypassProxy)),
                    Task<string>.Factory.StartNew(() => GetIPFromUrl("http://bot.whatismyipaddress.com/", bypassProxy))
                };

            do
            {
                // Wait for the first response and see see if we got an actual IP address back
                int index = Task.WaitAny(tasks.ToArray());
                string ipAddress = tasks[index].Result;

                if (!string.IsNullOrWhiteSpace(ipAddress))
                {
                    return ipAddress;
                }

                // Since we didn't get an IP back, try again, but without the task that's already finished
                tasks.RemoveAt(index);
            } while (tasks.Count > 0);

            // If none of the services could get our IP, try again without bypassing the proxy check
            string foundIpAddress = !bypassProxy ? "Unknown" : GetPublicIPAddress(false);
            log.InfoFormat("Public IP address retrieved for this machine was {0}", foundIpAddress);
            return foundIpAddress;
        }

        /// <summary>
        /// Gets the public ip address of this computer
        /// </summary>
        /// <param name="requestUrl">Url of the service that we'll use to get the IP address</param>
        /// <param name="bypassProxy">Should the request bypass the proxy check?</param>
        /// <param name="ipRegex">Regular expression to use to extract the IP address from the response</param>
        private string GetIPFromUrl(string requestUrl, bool bypassProxy, string ipRegex = @"(\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})")
        {
            try
            {
                WebRequest request = WebRequest.Create(requestUrl);

                // If we're bypassing the proxy check, the request should go quick.  If not, it will be slower
                if (bypassProxy)
                {
                    request.Timeout = 1000;
                    request.Proxy = null;
                }
                else
                {
                    request.Timeout = 5000;
                }

                // Make the actual request
                using (WebResponse response = request.GetResponse())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string text = reader.ReadToEnd();
                        Regex addressRegex = new Regex(ipRegex);
                        string ipAddress = addressRegex.Match(text).Groups[1].Value;

                        log.DebugFormat("IP Address retrieved from {0} was {1}", requestUrl, ipAddress);
                        return ipAddress;
                    }
                }
            }
            catch (Exception)
            {
                // Eat all exceptions, hopefully one of the other requests will succeed
                return string.Empty;
            }
        }
    }
}