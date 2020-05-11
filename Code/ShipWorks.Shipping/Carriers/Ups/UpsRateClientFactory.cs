using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// The factory for creating UPS rate clients
    /// </summary>
    [Component]
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
                ratingMethod = account.LocalRatingEnabled ?
                    UpsRatingMethod.LocalWithApiFailover :
                    UpsRatingMethod.ApiOnly;
            }
            return upsRateClientFactory[ratingMethod];
        }

        /// <summary>
        /// Get a UPS rate client by rating method
        /// </summary>
        public IUpsRateClient GetClient(UpsRatingMethod ratingMethod)
        {
            return upsRateClientFactory[ratingMethod];
        }
    }
}