using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing of a
    /// FedEx shipment 
    /// </summary>
    public class FedExShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public FedExShipmentProcessingSynchronizer()
            : this(new FedExAccountRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FedExShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public FedExShipmentProcessingSynchronizer(ICarrierAccountRepository<FedExAccountEntity, IFedExAccountEntity> accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// Gets a value indicating whether the shipment type [has accounts].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [has accounts]; otherwise, <c>false</c>.
        /// </value>
        public bool HasAccounts
        {
            get { return accountRepository.Accounts.Any(); }
        }

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new 
        /// account was just created during the processing of a shipment, so we just want 
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.FedEx.FedExAccountID = accountRepository.Accounts.First().FedExAccountID;
            }
            else
            {
                throw new FedExException("A FedEx account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && accountRepository.GetAccount(shipment.FedEx.FedExAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}
