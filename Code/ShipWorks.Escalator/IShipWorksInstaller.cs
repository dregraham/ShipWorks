using Interapptive.Shared.Utility;

namespace ShipWorks.Escalator
{
    /// <summary>
    /// Installs ShipWorks
    /// </summary>
    public interface IShipWorksInstaller
    {
        /// <summary>
        /// Installs ShipWorks
        /// </summary>
        Result Install(InstallFile file, bool upgradeDatabase, bool killShipWorksUI);
    }
}