using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Shipping.Carriers.OnTrac
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing
    /// of an OnTrac shipment
    /// </summary>
    public class OnTracShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public OnTracShipmentProcessingSynchronizer()
            : this(new OnTracAccountRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="OnTracShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public OnTracShipmentProcessingSynchronizer(ICarrierAccountRepository<OnTracAccountEntity, IOnTracAccountEntity> accountRepository)
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
        /// <exception cref="OnTracException">An OnTrac account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.OnTrac.OnTracAccountID = accountRepository.Accounts.First().OnTracAccountID;
            }
            else
            {
                throw new OnTracException("An OnTrac account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && accountRepository.GetAccount(shipment.OnTrac.OnTracAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}
