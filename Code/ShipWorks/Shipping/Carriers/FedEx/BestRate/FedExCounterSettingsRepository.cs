using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Environment;

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
    }
}
