using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Dashboard Item that prompts a user to upgrade due to shipping limit.
    /// </summary>
    public class DashboardLicenseItem : DashboardItem
    {
        /// <summary>
        /// Initialize the item with given bar that it will display its information in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            DashboardBar.CanUserDismiss = false;

            DashboardBar.PrimaryText = "Approaching Shipment Limit";
            DashboardBar.SecondaryText = "To continue shipping, please upgrade your plan.";
            DashboardBar.ApplyActions(new List<DashboardAction>
            {
                new DashboardActionMethod("[link] Upgrade Plan[/link]", OnUpgradePlan)
            });
        }

        /// <summary>
        /// Called when [upgrade plan].
        /// </summary>
        private void OnUpgradePlan()
        {
            using (ILifetimeScope scope = IoC.BeginLifetimeScope())
            {
                IWebBrowserFactory webBrowser = scope.Resolve<IWebBrowserFactory>();
                webBrowser.Create(new Uri(CustomerLicense.UpgradeUrl), "Upgrade your account", null);
            }
        }
    }
}