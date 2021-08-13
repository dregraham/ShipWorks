using System.Collections.Generic;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Properties;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a single dashboard trial item
    /// </summary>
    public abstract class DashboardTrialItem : DashboardItem
    {
        protected readonly TrialDetails trialDetails;
        
        // So we know when something has changed
        protected bool wasExpired;
        protected int days = 100;
        
        /// <summary>
        /// Constructor
        /// </summary>
        protected DashboardTrialItem(TrialDetails trialDetails)
        {
            this.trialDetails = trialDetails;
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
        /// Whether or not the display should be updated
        /// </summary>
        protected abstract bool ShouldUpdate { get; }
        
        /// <summary>
        /// Secondary text to display when the user is in a trial that has expired
        /// </summary>
        protected abstract string TrialExpiredSecondaryText { get; }
        
        /// <summary>
        /// Secondary text to display when the user is in a trial that hasn't expired
        /// </summary>
        protected abstract string TrialDaysLeftSecondaryText { get; }
        
        /// <summary>
        /// Update the display of the trial information
        /// </summary>
        public void UpdateTrialDisplay()
        {
            var daysLeftInTrial = trialDetails.DaysLeftInTrial;
            var trialIsExpired = trialDetails.IsExpired;

            // See if the UI requires updating
            if (ShouldUpdate)
            {
                if (trialIsExpired)
                {
                    DashboardBar.Image = Resources.clock_stop;
                    DashboardBar.PrimaryText = "Trial Expired";
                    DashboardBar.SecondaryText = TrialExpiredSecondaryText;
                }
                else
                {
                    DashboardBar.Image = Resources.clock;
                    DashboardBar.PrimaryText = $"{daysLeftInTrial} Days";
                    DashboardBar.SecondaryText = TrialDaysLeftSecondaryText;
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