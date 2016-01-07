namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for customer license
    /// </summary>
    public interface ICustomerLicense
    {
        /// <summary>
        /// Activate a customer license
        /// </summary>
        void Activate(string email, string password);

        /// <summary>
        /// The customer license key
        /// </summary>
        string Key { get; set; }
    }
}