﻿using System.Linq;
using ShipWorks.Data.Model.Custom.EntityClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.BestRate
{
    /// <summary>
    /// Counter rate broker that converts shipments to USPS (Stamps.com Expedited)
    /// </summary>
    public class UspsCounterRatesBroker : WebToolsCounterRatesBroker
    {
        private readonly ICarrierAccountRepository<StampsAccountEntity> stampsAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountRepository">Repository that will be used to access Stamps accounts</param>
        public UspsCounterRatesBroker(ICarrierAccountRepository<StampsAccountEntity> accountRepository) :
            base(new UspsShipmentType())
        {
            stampsAccountRepository = accountRepository;
        }

        /// <summary>
        /// Updates the shipment account with the actual account
        /// </summary>
        /// <param name="postalShipmentEntity"></param>
        /// <param name="account"></param>
        protected override void UpdateChildAccountId(PostalShipmentEntity postalShipmentEntity, NullEntity account)
        {
            StampsAccountEntity stampsAccount = stampsAccountRepository.Accounts.FirstOrDefault();

            if (postalShipmentEntity.Stamps == null)
            {
                postalShipmentEntity.Stamps = new StampsShipmentEntity();
            }

            if (stampsAccount != null)
            {
                postalShipmentEntity.Stamps.StampsAccountID = stampsAccount.StampsAccountID;
            }

            base.UpdateChildAccountId(postalShipmentEntity, account);
        }
    }
}
