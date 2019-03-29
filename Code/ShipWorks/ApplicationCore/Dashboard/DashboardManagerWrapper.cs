using Interapptive.Shared.ComponentRegistration;
using ShipWorks.ApplicationCore.Dashboard.Content;

namespace ShipWorks.ApplicationCore.Dashboard
{
    /// <summary>
    /// Dashboard manager wrapper
    /// </summary>
    [Component]
    public class DashboardManagerWrapper : IDashboardManager
    {
        /// <summary>
        /// Show a local dashboard message
        /// </summary>
        public void ShowLocalMessage(string identifier, DashboardMessageImageType imageType, string primaryText, string secondaryText, params DashboardAction[] actions) =>
            DashboardManager.ShowLocalMessage(identifier, imageType, primaryText, secondaryText, actions);
    }
}
