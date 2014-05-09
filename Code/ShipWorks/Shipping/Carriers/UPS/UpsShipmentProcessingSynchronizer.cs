﻿using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS
{
    public class UpsShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public UpsShipmentProcessingSynchronizer()
            : this(new UpsAccountRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpsShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public UpsShipmentProcessingSynchronizer(ICarrierAccountRepository<UpsAccountEntity> accountRepository)
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
        /// <exception cref="UpsException">An UPS account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
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
        /// If the account on the shipment is no longer valid, and there is only one account available,
        /// this method will change the account to be that one account.
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
