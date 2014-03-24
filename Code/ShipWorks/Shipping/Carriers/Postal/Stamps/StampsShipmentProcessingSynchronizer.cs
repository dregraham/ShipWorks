using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    public class StampsShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<StampsAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public StampsShipmentProcessingSynchronizer()
            : this(new StampsAccountRepository())
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="StampsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public StampsShipmentProcessingSynchronizer(ICarrierAccountRepository<StampsAccountEntity> accountRepository)
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
        /// <exception cref="StampsException">An Stamps account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.Postal.Stamps.StampsAccountID = accountRepository.Accounts.First().StampsAccountID;
            }
            else
            {
                throw new StampsException("A Stamps.com account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is only one account available,
        /// this method will change the account to be that one account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Count() == 1 && accountRepository.GetAccount(shipment.Postal.Stamps.StampsAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}