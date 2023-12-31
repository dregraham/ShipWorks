﻿using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Usps.BestRate;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1
{
    /// <summary>
    /// Best rate broker for Express1 USPS accounts
    /// </summary>
    public class Express1UspsBestRateBroker : UspsBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsBestRateBroker() : this(new Express1UspsShipmentType(), new Express1UspsAccountRepository(), BestRateExcludedAccountRepository.Current)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1UspsBestRateBroker(UspsShipmentType shipmentType, ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> accountRepository, IBestRateExcludedAccountRepository bestRateExcludedAccountRepository) :
            base(shipmentType, accountRepository, "USPS", bestRateExcludedAccountRepository)
        {

        }

        /// <summary>
        /// Get best rates for Express 1
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> exceptionHandler)
        {
            RateGroup rateGroup = base.GetBestRates(shipment, exceptionHandler);

            // Set the shipment type to be express1 for USPS
            rateGroup.Rates.ForEach(rr => rr.ShipmentType = ShipmentTypeCode.Express1Usps);

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
