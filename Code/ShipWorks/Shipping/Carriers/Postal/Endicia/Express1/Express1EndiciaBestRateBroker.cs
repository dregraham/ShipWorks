using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Best rate broker for Express1 Endicia accounts
    /// </summary>
    public class Express1EndiciaBestRateBroker : EndiciaBestRateBroker
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaBestRateBroker() : this(new Express1EndiciaShipmentType(), new Express1EndiciaAccountRepository())
        {
            
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Express1EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Get best rates for Express 1
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, Action<BrokerException> exceptionHandler)
        {
            RateGroup rateGroup = base.GetBestRates(shipment, exceptionHandler);

            RemoveInvalidExpress1Rates(rateGroup);

            // Set the shipment type to express1 for endicia
            rateGroup.Rates.ForEach(rr => rr.ShipmentType = ShipmentTypeCode.Express1Endicia);

            return rateGroup;
		}

        /// <summary>
        /// Remove rates for services that are not offered by Express 1
        /// </summary>
        private static void RemoveInvalidExpress1Rates(RateGroup rateGroup)
        {
            List<RateResult> invalidRates = rateGroup.Rates.Where(x => !IsValidExpress1Rate(x)).ToList();

            foreach (RateResult rate in invalidRates)
            {
                rateGroup.Rates.Remove(rate);
            }
        }

        /// <summary>
        /// Check whether the specified rate is valid for Express 1
        /// </summary>
        private static bool IsValidExpress1Rate(RateResult rate)
        {
            BestRateResultTag tag = rate.Tag as BestRateResultTag;
            if (tag != null)
            {
                PostalRateSelection postalTag = tag.OriginalTag as PostalRateSelection;
                if (postalTag != null)
                {
                    return Express1Utilities.IsPostageSavingService(postalTag.ServiceType);
                }
            }

            // If we couldn't get the tags cast correctly, then this rate certainly doesn't belong in here
            return false;
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
        protected override string AccountDescription(EndiciaAccountEntity account)
        {
            return account.Description;
        }
    }
}
