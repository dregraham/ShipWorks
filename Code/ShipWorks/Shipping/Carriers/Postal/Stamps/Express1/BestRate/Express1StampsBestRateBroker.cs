using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Stamps.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1.BestRate
{
    /// <summary>
    /// Best rate broker for Express1 Stamps accounts
    /// </summary>
    public class Express1StampsBestRateBroker : UspsBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsBestRateBroker() : this(new Express1StampsShipmentType(), new Express1StampsAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Get best rates for Express 1
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> exceptionHandler)
        {
            RateGroup rateGroup = base.GetBestRates(shipment, exceptionHandler);
            
            // Set the shipment type to be express1 for stamps
            rateGroup.Rates.ForEach(rr => rr.ShipmentType = ShipmentTypeCode.Express1Stamps);

            return rateGroup;
        }

        /// <summary>
        /// Configures extra settings required by the broker
        /// </summary>
        /// <param name="brokerSettings">Settings that the broker can use to configure itself</param>
        public override void Configure(IBestRateBrokerSettings brokerSettings)
        {
            // Don't configure Express1 settings
        }

        /// <summary>
        /// Gets a description from the specified account
        /// </summary>
        protected override string AccountDescription(UspsAccountEntity account)
        {
            return account.Description;
        }
    }
}
