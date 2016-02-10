namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Interface for UspsAccountManager
    /// </summary>
    public interface IUspsAccountManager
    {
        /// <summary>
        /// Initializes for current session.
        /// </summary>
        void InitializeForCurrentSession();
    }
}