using System;
using System.Collections.Generic;
using ShipWorks.Properties;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a single dashboard item representing a store trial
    /// </summary>
    public class DashboardTrialItem : DashboardItem
    {
        private DateTime trialEndDate;
        
        // So we know when something has changed
        private bool wasExpired = false;
        private int days = 100;
        private string storeName;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardTrialItem(IStoreEntity store, DateTime trialEndDate)
        {
            Store = store;
            this.trialEndDate = trialEndDate;
        }

        /// <summary>
        /// Store associated with the trial
        /// </summary>
        public IStoreEntity Store { get; }

        /// <summary>
        /// Set the bar that the trial information will be displayed in
        /// </summary>
        public override void Initialize(DashboardBar dashboardBar)
        {
            base.Initialize(dashboardBar);

            dashboardBar.Image = Resources.clock;
            dashboardBar.CanUserDismiss = false;

            UpdateTrialDisplay();
        }

        /// <summary>
        /// Update the display of the trial information
        /// </summary>
        public void UpdateTrialDisplay()
        {
            var daysLeftInTrial = (trialEndDate - DateTime.UtcNow).Days;
            var trialIsExpired = daysLeftInTrial < 0;

            // See if the UI requires updating
            if (storeName != Store.StoreName ||
                days != daysLeftInTrial ||
                wasExpired != trialIsExpired)
            {
                if (trialIsExpired)
                {
                    DashboardBar.Image = Resources.clock_stop;
                    DashboardBar.PrimaryText = "Trial Expired";
                    DashboardBar.SecondaryText = $"The trial for '{Store.StoreName}' has expired.";
                }
                else
                {
                    DashboardBar.Image = Resources.clock;
                    DashboardBar.PrimaryText = $"{daysLeftInTrial} Days";
                    DashboardBar.SecondaryText = $"remaining in trial for '{Store.StoreName}'.";
                }

                DashboardBar.ApplyActions(new List<DashboardAction> {
                    new DashboardActionUrl(
                        "[link]Add a credit card[/link].", 
                        "https://hub.shipworks.com/account") });

                storeName = Store.StoreName;
                days = daysLeftInTrial;
                wasExpired = trialIsExpired;
            }
        }
    }
}
