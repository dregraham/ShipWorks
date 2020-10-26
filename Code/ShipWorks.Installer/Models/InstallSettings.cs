using System.Reflection;
using GalaSoft.MvvmLight;


namespace ShipWorks.Installer.Models
{
    /// <summary>
    /// DTO for current install settings
    /// </summary>
    [Obfuscation]
    public class InstallSettings : ObservableObject
    {
        private SystemCheckResult checkSystemResult;
        private string installPath;
        private bool createShortcut;

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
        public string InstallPath
        {
            get => installPath;
            set => Set(ref installPath, value);
        }

        /// <summary>
        /// Whether or not to create a shortcut on the desktop
        /// </summary>
        public bool CreateShortcut
        {
            get => createShortcut;
            set => Set(ref createShortcut, value);
        }
    }
}
