namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public interface IGlobalPostLabelNotification
    {
        /// <summary>
        /// Check to see if we should show the notification based on the current user.
        /// </summary>
        bool AppliesToCurrentUser();

        /// <summary>
        /// Show the notification and save result
        /// </summary>
        void Show();
    }
}