using System.Reflection;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using ShipWorks.Installer.Api.DTO;
using ShipWorks.Installer.Enums;

namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// DTO for current install settings
    /// </summary>
    [Obfuscation]
    public class InstallSettings : ObservableObject
    {
        private SystemCheckResult checkSystemResult;

        /// <summary>
        /// Results from system check
        /// </summary>
        public SystemCheckResult CheckSystemResult
        {
            get => checkSystemResult;
            set => Set(ref checkSystemResult, value);
        }

        /// <summary>
        /// The installation path
        /// </summary>
        public string InstallPath { get; set; }

        /// <summary>
        /// Whether or not to create a shortcut on the desktop
        /// </summary>
        public bool CreateShortcut { get; set; }

        /// <summary>
        /// Whether or not the INNO setup installer has finished downloading
        /// </summary>
        public bool InnoSetupDownloaded { get; set; } = false;

        /// <summary>
        /// The db connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// The warehouse that we should pull config data from
        /// </summary>
        public Warehouse Warehouse { get; set; }

        /// <summary>
        /// The type of error that occurred during installation
        /// </summary>
        public InstallError Error { get; set; } = InstallError.None;

        /// <summary>
        /// If an installation error occurred, this is the message
        /// </summary>
        public string AutoInstallErrorMessage { get; set; }

        /// <summary>
        /// The token for authorizing Hub requests
        /// </summary>
        public HubToken Token { get; set; }

        /// <summary>
        /// The tango email address to use
        /// </summary>
        public string TangoEmail { get; set; }

        /// <summary>
        /// The tango password to use
        /// </summary>
        [JsonIgnore]
        public string TangoPassword { get; set; }
    }
}
