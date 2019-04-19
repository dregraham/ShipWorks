using System.Windows.Forms;

namespace ShipWorks.ApplicationCore.Settings
{
    /// <summary>
    /// Interface for a settings page
    /// </summary>
    public interface ISettingsPage
    {
        /// <summary>
        /// Settings page control
        /// </summary>
        Control Control { get;  }

        /// <summary>
        /// Save the page
        /// </summary>
        void Save();
    }
}