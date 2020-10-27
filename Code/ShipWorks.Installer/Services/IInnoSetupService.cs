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
    }
}