using ShipWorks.Stores.Platforms.Amazon;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    public interface IAmazonAccountValidator
    {
        /// <summary>
        /// Checks account to see if it is valid
        /// </summary>
        bool ValidateAccount(IAmazonCredentials credentials);
    }
}
