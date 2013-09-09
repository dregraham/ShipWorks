namespace Interapptive.Shared.Net
{
    public interface INetworkUtility
    {
        /// <summary>
        /// Gets the IP address.
        /// </summary>
        /// <returns>
        /// A version 4 IP address.
        /// </returns>
        /// <exception cref="Interapptive.Shared.Net.NetworkException">ShipWorks could not obtain the IP address of this machine.</exception>
        string GetIPAddress();
    }
}