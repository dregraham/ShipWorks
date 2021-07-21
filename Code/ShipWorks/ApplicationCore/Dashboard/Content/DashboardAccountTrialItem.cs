using System;
using System.Collections.Generic;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    public class DashboardAccountTrialItem : DashboardItem
    {
        private DateTime trialEndDate;
        
        // So we know when something has changed
        private bool wasExpired = false;
        private int days = 100;
        private string storeName;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardAccountTrialItem(DateTime trialEndDate)
        {
            this.trialEndDate = trialEndDate;
        }

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
            if (days != daysLeftInTrial ||
                wasExpired != trialIsExpired)
            {
                if (trialIsExpired)
                {
                    DashboardBar.Image = Resources.clock_stop;
                    DashboardBar.PrimaryText = "Trial Expired";
                    DashboardBar.SecondaryText = "The trial for your ShipWorks subscription has expired.";
                }
                else
                {
                    DashboardBar.Image = Resources.clock;
                    DashboardBar.PrimaryText = $"{daysLeftInTrial} Days";
                    DashboardBar.SecondaryText = "remaining in trial for your ShipWorks subscription.";
                }

                DashboardBar.ApplyActions(new List<DashboardAction> {
                    new DashboardActionUrl(
                        "[link]Click here[/link] to login to your account settings and enter your payment details.", 
                        "https://hub.shipworks.com/account") });

                days = daysLeftInTrial;
                wasExpired = trialIsExpired;
            }
        }
    }
}