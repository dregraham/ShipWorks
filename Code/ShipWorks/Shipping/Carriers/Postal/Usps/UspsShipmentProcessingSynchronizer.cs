using System.Linq;
using Interapptive.Shared.Collections;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    public class UspsShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UspsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public UspsShipmentProcessingSynchronizer(ICarrierAccountRepository<UspsAccountEntity> accountRepository)
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
            get
            {
                // Checking for pending initial accounts here as well, so the
                // a label can't be processed with an account in the pending state.
                return accountRepository.Accounts.Any() &&
                       accountRepository.Accounts.All(a => a.PendingInitialAccount == (int) UspsPendingAccountType.None);
            }
        }

        /// <summary>
        /// Saves the first account ID found to the shipment. The presumption here is that a new
        /// account was just created during the processing of a shipment, so we just want
        /// to grab the first account (the one just created) and use it to process the shipment
        /// </summary>
        /// <param name="shipment">The shipment.</param>
        /// <exception cref="UspsException">A USPS account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.Postal.Usps.UspsAccountID = accountRepository.Accounts.First().UspsAccountID;
            }
            else
            {
                throw new UspsException("An account must be created to process this shipment.");
            }
        }

        /// <summary>
        /// If the account on the shipment is no longer valid, and there is an account available,
        /// this method will change the account to be the first account.
        /// </summary>
        public void ReplaceInvalidAccount(ShipmentEntity shipment)
        {
            if (HasAccounts && shipment.Postal.Usps != null && accountRepository.GetAccount(shipment.Postal.Usps.UspsAccountID) == null)
            {
                SaveAccountToShipment(shipment);
            }
        }
    }
}