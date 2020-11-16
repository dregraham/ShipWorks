using System.Threading.Tasks;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Service for calling ShipWorks command line options
    /// </summary>
    public interface IShipWorksCommandLineService
    {
        /// <summary>
        /// Call ShipWorks to install LocalDB, create the db, create the user, link the warehouse, etc...
        /// </summary>
        Task<int> AutoInstallShipWorks(InstallSettings installSettings);
    }
}
