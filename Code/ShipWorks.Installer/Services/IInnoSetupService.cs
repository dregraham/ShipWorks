using System.Threading.Tasks;
using ShipWorks.Installer.Models;

namespace ShipWorks.Installer.Services
{
    /// <summary>
    /// Interface for downloading and running the INNO Setup Installer
    /// </summary>
    public interface IInnoSetupService
    {
        /// <summary>
        /// Download the INNO Setup installer
        /// </summary>
        void DownloadInstaller(InstallSettings installSettings);

        /// <summary>
        /// Call Inno installer to install ShipWorks
        /// </summary>
        Task InstallShipWorks(InstallSettings installSettings);

        /// <summary>
        /// Silently run the INNO uninstaller
        /// </summary>
        Task RunUninstaller();
    }
}