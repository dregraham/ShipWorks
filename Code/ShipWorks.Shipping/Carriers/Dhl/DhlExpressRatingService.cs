using System;
using System.Linq;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;
using ShipEngine.ApiClient.Model;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express rating service
    /// </summary>
    [KeyedComponent(typeof(IRatingService), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressRatingService : IRatingService
    {
        private readonly DhlExpressShipEngineRatingClient shipEngineRatingClient;
        private readonly DhlExpressStampsRatingClient stampsRatingClient;
        private readonly IDhlExpressAccountRepository accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressRatingService(
            DhlExpressShipEngineRatingClient shipEngineRatingClient,
            DhlExpressStampsRatingClient stampsRatingClient,
            IDhlExpressAccountRepository accountRepository)
        {

            this.shipEngineRatingClient = shipEngineRatingClient;
            this.stampsRatingClient = stampsRatingClient;
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Get rates from DHL Express via ShipEngine
        /// </summary>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            // We don't have any DHL Express accounts, so let the user know they need an account.
            if (!accountRepository.Accounts.Any())
            {
                throw new ShippingException("An account is required to view DHL Express rates.");
            }

            try
            {
                IDhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

                return account?.ShipEngineCarrierId == null ?
                    stampsRatingClient.GetRates(shipment) :
                    shipEngineRatingClient.GetRates(shipment);
            }
            catch (Exception ex)
            {
                throw new ShippingException(ex);
            }
        }
    }
}
