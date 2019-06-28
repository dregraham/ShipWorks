using System;
using System.Collections.Generic;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// An item in the dashboard that shows that there is a new version of ShipWorks available
    /// </summary>
    internal class DashboardOnlineVersionItem : DashboardItem
    {
        private Version onlineVersion;
        private DateTime updateWindow;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardOnlineVersionItem(Version onlineVersion, DateTime updateWindow)
        {
            this.updateWindow = updateWindow;
            this.onlineVersion = onlineVersion;
        }

        /// <summary>
        /// Set the date that will be displayed for the update window
        /// </summary>
        public void CopyFrom(DashboardOnlineVersionItem item)
        {
            updateWindow = item.updateWindow;
            onlineVersion = item.onlineVersion;

            UpdateVersionDisplay();
        }

        /// <summary>
        /// Set the dashboard bar that the item will display its content in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            dashboardBar.Image = Resources.box_software;
            dashboardBar.SecondaryText = string.Empty;

            UpdateVersionDisplay();
        }

        /// <summary>
        /// Update the version displayed in the bar
        /// </summary>
        private void UpdateVersionDisplay()
        {
            var now = DateTime.Now;
            var timeUntilUpdateWindow = updateWindow.Subtract(now);

            // Make sure we never say "1 days"
            if (Math.Round(timeUntilUpdateWindow.TotalDays) <= 1)
            {
                DashboardBar.PrimaryText = $"ShipWorks is scheduled to update on {updateWindow.ToString("MMMM d")} at {updateWindow.ToString("h:00 tt")}. ";
            }
            else
            {
                DashboardBar.PrimaryText = $"ShipWorks is scheduled to update in {Math.Round(timeUntilUpdateWindow.TotalDays)} days. ";
            }

            DashboardBar.ApplyActions(new List<DashboardAction> {
                new DashboardActionUrl("[link]Click here[/link] to learn more.",
                "https://support.shipworks.com/hc/en-us/articles/360025461732-All-About-ShipWorks-AutoUpdate") });
        }

        /// <summary>
        /// The dashboard item is being dismissed
        /// </summary>
        public override void Dismiss()
        {
            base.Dismiss();

            // Signoff on having seen this version
            ShipWorksOnlineVersionChecker.Signoff(onlineVersion);
        }
    }
}
