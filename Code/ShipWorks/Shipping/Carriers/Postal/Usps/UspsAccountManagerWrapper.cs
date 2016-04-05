namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Wrapper for UspsAccountManager
    /// </summary>
    public class UspsAccountManagerWrapper : IUspsAccountManager
    {
        /// <summary>
        /// Initializes for current session.
        /// </summary>
        public void InitializeForCurrentSession()
        {
            UspsAccountManager.InitializeForCurrentSession();
        }
    }
}