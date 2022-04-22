using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.DhlEcommerce
{
    /// <summary>
    /// Dhl eCommerce rating service
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.DhlEcommerce)]
    public class DhlEcommerceRatingService : IRatingService
    {
        private readonly IDhlEcommerceShipEngineRatingClient shipEngineRatingClient;
        private readonly IDhlEcommerceAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceRatingService(
            IDhlEcommerceShipEngineRatingClient shipEngineRatingClient,
            IDhlEcommerceAccountRepository accountRepository)
        {

            this.shipEngineRatingClient = shipEngineRatingClient;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get rates from DHL eCommerce via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any DHL eCommerce accounts, so let the user know they need an account.
            if (!accountRepository.AccountsReadOnly.Any())
            {
                throw new ShippingException("An account is required to view DHL eCommerce rates.");
            }

            try
            {
                IDhlEcommerceAccountEntity account = accountRepository.GetAccountReadOnly(shipment);

                return shipEngineRatingClient.GetRates(shipment);
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex);
            }
        }
    }
}
