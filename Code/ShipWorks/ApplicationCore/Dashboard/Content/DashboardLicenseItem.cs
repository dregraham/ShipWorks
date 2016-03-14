using System;
using System.Collections.Generic;
using System.Windows;
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
        private readonly float currentShipmentPercentage;

        public DashboardLicenseItem(DateTime billingEndDate, float currentShipmentPercentage)
        {
            this.billingEndDate = billingEndDate;
            this.currentShipmentPercentage = currentShipmentPercentage;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is under shipment limit.
        /// </summary>
        /// <value><c>true</c> if this instance is under shipment limit; otherwise, <c>false</c>.</value>
        public bool IsUnderShipmentLimit
        {
            get { return currentShipmentPercentage < 1.0; }
        }

        /// <summary>
        /// Initialize the item with given bar that it will display its information in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            DashboardBar.CanUserDismiss = false;
            DashboardBar.PrimaryText = "Licensing";

            // Tailor the message to the depending on where the customer is at within their shipment
            DashboardBar.SecondaryText = IsUnderShipmentLimit 
                ? $"You are nearing your shipment limit for the current billing cycle ending {billingEndDate.ToString("M/d")}." 
                : $"You have reached your shipment limit for the current billing cycle ending {billingEndDate.ToString("M/d")}.";

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
                IDialog dialog = webBrowser.Create(new Uri(CustomerLicense.UpgradeUrl), "Upgrade your plan",
                    DashboardBar, new Size(1053, 1010));
                dialog.ShowDialog();
            }
        }
    }
}