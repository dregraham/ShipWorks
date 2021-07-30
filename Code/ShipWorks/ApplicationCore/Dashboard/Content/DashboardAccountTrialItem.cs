using ShipWorks.ApplicationCore.Licensing;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a single dashboard item representing a WebReg account trial
    /// </summary>
    public class DashboardAccountTrialItem : DashboardTrialItem
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardAccountTrialItem(TrialDetails trialDetails) : base(trialDetails)
        {
        }

        /// <summary>
        /// Whether or not the display should be updated
        /// </summary>
        protected override bool ShouldUpdate => days != trialDetails.DaysLeftInTrial || wasExpired != trialDetails.IsExpired;

        /// <summary>
        /// Secondary text to display when the user is in a trial that has expired
        /// </summary>
        protected override string TrialExpiredSecondaryText => "The trial for your ShipWorks subscription has expired.";

        /// <summary>
        /// Secondary text to display when the user is in a trial that hasn't expired
        /// </summary>
        protected override string TrialDaysLeftSecondaryText => "remaining in trial for your ShipWorks subscription.";
    }
}