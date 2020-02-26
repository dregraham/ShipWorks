using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;

namespace ShipWorks.Shipping.Carriers.Ups
{
    /// <summary>
    /// The factory for creating UPS rate clients
    /// </summary>
    public interface IUpsRateClientFactory
    {
        /// <summary>
        /// Get a UPS rate client
        /// </summary>
        IUpsRateClient GetClient(UpsAccountEntity account);
    }
}
