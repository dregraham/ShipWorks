using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.ApplicationCore.Help
{
    /// <summary>
    /// Interface for the AboutShipWorksDialog
    /// </summary>
    [Service]
    public interface IAboutShipWorksDialog
    {
        /// <summary>
        /// Show the dialog
        /// </summary>
        bool? ShowDialog();
    }
}