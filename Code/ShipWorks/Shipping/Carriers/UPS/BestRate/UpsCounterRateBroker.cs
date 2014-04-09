using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.UPS.OnLineTools;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    internal class UpsCounterRateBroker : UpsBestRateBroker
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRateBroker"/> class.
        /// </summary>
        /// <remarks>
        /// This is designed to be used within ShipWorks
        /// </remarks>
        public UpsCounterRateBroker()
            : this(new UpsOltShipmentType(), new UpsCounterRateAccountRepository(TangoCounterRatesCredentialStore.Instance), new UpsCounterRateSettingsRepository(TangoCounterRatesCredentialStore.Instance))
        {}


        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRateBroker" /> class.
        /// </summary>
        /// <param name="upsShipmentType">Type of the ups shipment.</param>
        /// <param name="accountRepository">The account repository.</param>
        /// <param name="upsCounterRateSettingsRepository">The ups counter rate settings repository.</param>
        private UpsCounterRateBroker(UpsShipmentType upsShipmentType, ICarrierAccountRepository<UpsAccountEntity> accountRepository, UpsSettingsRepository upsCounterRateSettingsRepository) :
            base(upsShipmentType, accountRepository, upsCounterRateSettingsRepository)
        {
        }



        /// <summary>
        /// Gets the first account from the repository and returns the ID.
        /// </summary>
        protected override long GetAccountID(UpsAccountEntity account)
        {
            return account.UpsAccountID;
        }

        /// <summary>
        /// Wraps the shipping exception.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <returns>A new BrokerException with a severity level of Information.</returns>
        protected override BrokerException WrapShippingException(ShippingException ex)
        {
            // Since this is just getting counter rates, we want to have the severity level
            // as information for all shipping exceptions
            return new BrokerException(ex, BrokerExceptionSeverityLevel.Information, ShipmentType);
        }
    }
}