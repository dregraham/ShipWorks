using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using System.Linq;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// Handles Pre-processing of Ups Shipments
    /// </summary>
    public class UpsShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly bool isShipmentTypeConfigured;
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public UpsShipmentProcessingSynchronizer(bool isShipmentTypeConfigured)
            : this(new UpsAccountRepository(), isShipmentTypeConfigured)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public UpsShipmentProcessingSynchronizer(ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository, bool isShipmentTypeConfigured)
        {
            this.accountRepository = accountRepository;
            this.isShipmentTypeConfigured = isShipmentTypeConfigured;
        }


        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts => isShipmentTypeConfigured && accountRepository.Accounts.Any();

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new
        /// account was just created during the processing of a shipment, so we just want
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="UpsException">An UPS account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (HasAccounts)
            {
                // Grab the first account in the repository to set the account ID
                shipment.Ups.UpsAccountID = accountRepository.Accounts.First().UpsAccountID;
            }
            else
            {
                throw new UpsException("An UPS account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && accountRepository.GetAccount(shipment.Ups.UpsAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}
