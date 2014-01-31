using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsCounterRateSettingsRepository : UpsSettingsRepository
    {
        private readonly TangoCounterRatesCredentialStore tangoCounterRatesCredentialStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRateSettingsRepository"/> class.
        /// </summary>
        /// <param name="tangoCounterRatesCredentialStore">The tango counter rates credential store.</param>
        public UpsCounterRateSettingsRepository(TangoCounterRatesCredentialStore tangoCounterRatesCredentialStore)
        {
            this.tangoCounterRatesCredentialStore = tangoCounterRatesCredentialStore;
        }

        /// <summary>
        /// Gets the shipping settings, overriding UpsAccessKey
        /// </summary>
        public override ShippingSettingsEntity GetShippingSettings()
        {
            ShippingSettingsEntity shippingSettingsEntity = base.GetShippingSettings();

            try
            {
                shippingSettingsEntity.UpsAccessKey = tangoCounterRatesCredentialStore.UpsAccessKey;
            }
            catch (MissingCounterRatesCredentialException)
            {
                // Eat this exception - and just use whatever is returned from the base class
            }

            return shippingSettingsEntity;
        }
    }
}