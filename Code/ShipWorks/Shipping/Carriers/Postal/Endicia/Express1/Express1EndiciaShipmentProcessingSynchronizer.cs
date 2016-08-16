using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing
    /// of an Express1 for Endicia shipment
    /// </summary>
    public class Express1EndiciaShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1EndiciaShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public Express1EndiciaShipmentProcessingSynchronizer()
            : this(new Express1EndiciaAccountRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Express1EndiciaShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public Express1EndiciaShipmentProcessingSynchronizer(ICarrierAccountRepository<EndiciaAccountEntity, IEndiciaAccountEntity> accountRepository)
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
        /// <exception cref="Express1EndiciaException">An Express1 account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.Postal.Endicia.EndiciaAccountID = accountRepository.Accounts.First().EndiciaAccountID;
            }
            else
            {
                throw new Express1EndiciaException("An Express1 account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && shipment.Postal.Endicia != null && accountRepository.GetAccount(shipment.Postal.Endicia.EndiciaAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}
