
namespace ShipWorks.Shipping.Carriers.Postal.Express1
{
    /// <summary>
    /// An interface for saving an Express1 registration that abstracts the details of the underlying data source.
    /// This allows for ShipWorks to have one "registration" class even though the account data may actually get
    /// saved in a different table by injecting the appropriate implementation of this interface.
    /// </summary>
    public interface IExpress1RegistrationRepository
    {
        /// <summary>
        /// Saves the Express1 registration to the appropriate data source (Stamps account table or Endicia account table).
        /// </summary>
        /// <param name="registration">The registration object containing the Express1 account info being saved.</param>
        void Save(Express1Registration registration);
    }
}
