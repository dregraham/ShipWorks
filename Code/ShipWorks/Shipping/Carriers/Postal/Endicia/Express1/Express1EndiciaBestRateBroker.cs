using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal.Endicia.BestRate;
using ShipWorks.Shipping.Editing.Rating;

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
        public Express1EndiciaBestRateBroker(EndiciaShipmentType shipmentType, ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository) :
            base(shipmentType, accountRepository, "USPS")
        {

        }

        /// <summary>
        /// Get best rates for Express 1
        /// </summary>
        public override RateGroup GetBestRates(ShipmentEntity shipment, List<BrokerException> brokerExceptions)
        {
            RateGroup rateGroup = base.GetBestRates(shipment, brokerExceptions);

            // Set the shipment type to express1 for endicia
            rateGroup.Rates.ForEach(rr => rr.ShipmentType = ShipmentTypeCode.Express1Endicia);

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
        protected override string AccountDescription(EndiciaAccountEntity account)
        {
            return account.Description;
        }
    }
}
