using System;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.UpsEnvironment;

namespace ShipWorks.Shipping.Carriers.UPS.BestRate
{
    public class UpsCounterRateSettingsRepository : UpsSettingsRepository
    {
        private readonly TangoCredentialStore tangoCredentialStore;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsCounterRateSettingsRepository"/> class.
        /// </summary>
        /// <param name="tangoCredentialStore">The tango counter rates credential store.</param>
        public UpsCounterRateSettingsRepository(TangoCredentialStore tangoCredentialStore)
        {
            this.tangoCredentialStore = tangoCredentialStore;
        }

        /// <summary>
        /// Gets the shipping settings, overriding UpsAccessKey
        /// </summary>
        public override ShippingSettingsEntity GetShippingSettings()
        {
            ShippingSettingsEntity shippingSettingsEntity = base.GetShippingSettings();

            try
            {
                shippingSettingsEntity.UpsAccessKey = tangoCredentialStore.UpsAccessKey;
            }
            catch (MissingCounterRatesCredentialException)
            {
                // Eat this exception - and just use whatever is returned from the base class
            }

            return shippingSettingsEntity;
        }
    }
}