using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.ApplicationCore.Dashboard.Content
{
    /// <summary>
    /// Represents a single dashboard item representing a store trial
    /// </summary>
    public class DashboardLegacyStoreTrialItem : DashboardTrialItem
    {
        private string storeName;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardLegacyStoreTrialItem(IStoreEntity store, TrialDetails trialDetails) : base(trialDetails)
        {
            Store = store;
        }

        /// <summary>
        /// Store associated with the trial
        /// </summary>
        public IStoreEntity Store { get; }


        /// <summary>
        /// Whether or not the display should be updated
        /// </summary>
        protected override bool ShouldUpdate => storeName != Store.StoreName ||
                                                days != trialDetails.DaysLeftInTrial ||
                                                wasExpired != trialDetails.IsExpired;

        /// <summary>
        /// Secondary text to display when the user is in a trial that has expired
        /// </summary>
        protected override string TrialExpiredSecondaryText => $"The trial for '{Store.StoreName}' has expired.";

        /// <summary>
        /// Secondary text to display when the user is in a trial that hasn't expired
        /// </summary>
        protected override string TrialDaysLeftSecondaryText => $"remaining in trial for '{Store.StoreName}'.";

        /// <summary>
        /// Update the display of the trial information
        /// </summary>
        public void UpdateTrialDisplay()
        {
            base.UpdateTrialDisplay();

            if (!storeName.Equals(Store.StoreName))
            {
                storeName = Store.StoreName;
            }
        }
    }
}
