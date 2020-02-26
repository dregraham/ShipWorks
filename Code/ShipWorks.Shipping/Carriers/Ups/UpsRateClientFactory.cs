using Autofac.Features.Indexed;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// The factory for creating UPS rate clients
    /// </summary>
    public class UpsRateClientFactory : IUpsRateClientFactory
    {
        private readonly IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsRateClientFactory(IIndex<UpsRatingMethod, IUpsRateClient> upsRateClientFactory)
        {
            this.upsRateClientFactory = upsRateClientFactory;
        }

        /// <summary>
        /// Get a UPS rate client
        /// </summary>
        public IUpsRateClient GetClient(UpsAccountEntity account)
        {
            UpsRatingMethod ratingMethod;

            if (account == null)
            {
                ratingMethod = UpsRatingMethod.ApiOnly;
            }
            else
            {
                if (account.ShipEngineCarrierId != null)
                {
                    ratingMethod = UpsRatingMethod.ShipEngine;
                }
                else
                {
                    ratingMethod = account.LocalRatingEnabled ? 
                        UpsRatingMethod.LocalWithApiFailover : 
                        UpsRatingMethod.ApiOnly;
                }
            }
            return upsRateClientFactory[ratingMethod];
        }
    }
}