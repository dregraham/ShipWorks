using System;
using System.Threading.Tasks;
using Interapptive.Shared.AutoUpdate;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Upgrade ShipWorks
    /// </summary>
    public interface IShipWorksUpgrade
    {
        /// <summary>
        /// Download and install the next version of ShipWorks for the given customer if it is available
        /// </summary>
        Task Upgrade(string tangoCustomerId);

        /// <summary>
        /// Upgrade Shipworks to the requested version
        /// </summary>
        Task Upgrade(UpgradeToVersion version);
    }
}