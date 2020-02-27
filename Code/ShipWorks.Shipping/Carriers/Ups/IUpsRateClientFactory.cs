using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

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

        /// <summary>
        /// Get a UPS rate client by rating method
        /// </summary>
        IUpsRateClient GetClient(UpsRatingMethod ratingMethod);
    }
}
