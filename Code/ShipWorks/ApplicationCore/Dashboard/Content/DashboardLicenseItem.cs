using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Properties;
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
            base.Initialize(dashboardBar);

            DashboardBar.CanUserDismiss = false;

            DashboardBar.PrimaryText = "Licensing";
            DashboardBar.SecondaryText = "You are nearing your monthly shipment limit.";
            DashboardBar.Image = Resources.warning16;
            DashboardBar.ApplyActions(new List<DashboardAction>
            {
                new DashboardActionMethod("[link]Upgrade your plan[/link] now", OnUpgradePlan)
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
                webBrowser.Create(new Uri(CustomerLicense.UpgradeUrl), "Upgrade your plan", null);
            }
        }
    }
}