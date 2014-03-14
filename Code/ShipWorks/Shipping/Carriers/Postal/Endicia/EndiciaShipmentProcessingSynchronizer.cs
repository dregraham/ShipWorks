﻿using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// An IShipmentProcessingSynchronizer implementation to handle the PreProcessing 
    /// of an Endicia shipment 
    /// </summary>
    public class EndiciaShipmentProcessingSynchronizer : IShipmentProcessingSynchronizer
    {
        private readonly ICarrierAccountRepository<EndiciaAccountEntity> accountRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaShipmentProcessingSynchronizer"/> class.
        /// </summary>
        public EndiciaShipmentProcessingSynchronizer()
            : this(new EndiciaAccountRepository())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EndiciaShipmentProcessingSynchronizer"/> class.
        /// </summary>
        /// <param name="accountRepository">The account repository.</param>
        public EndiciaShipmentProcessingSynchronizer(ICarrierAccountRepository<EndiciaAccountEntity> accountRepository)
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
        /// <exception cref="EndiciaException">An Endicia account must be created to process this shipment.</exception>
        public void SaveAccountToShipment(ShipmentEntity shipment)
        {
            if (accountRepository.Accounts.Any())
            {
                // Grab the first account in the repository to set the account ID
                shipment.Postal.Endicia.EndiciaAccountID = accountRepository.Accounts.First().EndiciaAccountID;
            }
            else
            {
                throw new EndiciaException("An Endicia account must be created to process this shipment.");
            }
        }
    }
}
