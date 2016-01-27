namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for customer license
    /// </summary>
    public interface ICustomerLicense : ILicense
    {
        /// <summary>
        /// Saves the license
        /// </summary>
        void Save();

        /// <summary>
        /// Activate a customer license
        /// </summary>
        void Activate(string email, string password);
    }
}