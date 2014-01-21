using System.Collections.Generic;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;

namespace ShipWorks.Shipping.Carriers.FedEx.BestRate
{
    /// <summary>
    /// A FedEx counter implementation of the ICarrierSettingsRepository interface. This communicates with external
    /// dependencies/data stores such as the ShipWorks database and the Windows registry.
    /// </summary>
    public class FedExCounterSettingsRepository : FedExSettingsRepository
    {
        private readonly ICounterRatesCredentialStore credentialStore;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExCounterSettingsRepository(ICounterRatesCredentialStore credentialStore)
        {
            this.credentialStore = credentialStore;
        }

        /// <summary>
        /// Gets shipping settings with the counter version of the FedEx credentials
        /// </summary>
        /// <returns></returns>
        public override ShippingSettingsEntity GetShippingSettings()
        {
            ShippingSettingsEntity settings = base.GetShippingSettings();
            settings.FedExUsername = credentialStore.FedExUsername;
            settings.FedExPassword = credentialStore.FedExPassword;
            return settings;
        }

        /// <summary>
        /// Gets the FedEx account that should be used for counter rates.
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A FedExAccountEntity object.</returns>
        public override IEntity2 GetAccount(ShipmentEntity shipment)
        {
            return new FedExAccountEntity
            {
                AccountNumber = credentialStore.FedExAccountNumber,
                MeterNumber = credentialStore.FedExMeterNumber
            };
        }

        /// <summary>
        /// Gets a list of the FedEx account that should be used for counter rates
        /// </summary>
        public override IEnumerable<IEntity2> GetAccounts()
        {
            return new List<IEntity2> { GetAccount(null) };
        }
    }
}
