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
        private readonly DateTime billingEndDate;

        public DashboardLicenseItem(DateTime billingEndDate)
        {
            this.billingEndDate = billingEndDate;
        }

        /// <summary>
        /// Initialize the item with given bar that it will display its information in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.CanUserDismiss = false;

            DashboardBar.PrimaryText = "Licensing";
            DashboardBar.SecondaryText = $"You are nearing your shipment limit for the current billing cycle (ending {billingEndDate.ToString("d/M")}).";
            DashboardBar.Image = Resources.warning16;
            DashboardBar.ApplyActions(new List<DashboardAction>
            {
                new DashboardActionMethod("[link]Click here to upgrade your plan now.[/link]", OnUpgradePlan)
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
                IDialog dialog = webBrowser.Create(new Uri(CustomerLicense.UpgradeUrl), "Upgrade your plan", null);
                dialog.ShowDialog();
            }
        }
    }
}