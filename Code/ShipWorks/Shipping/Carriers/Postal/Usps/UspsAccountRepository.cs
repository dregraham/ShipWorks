﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Basic repository for retrieving USPS (Stamps.com Expedited) accounts
    /// </summary>
    public class UspsAccountRepository : CarrierAccountRepositoryBase<StampsAccountEntity>, ICarrierAccountRepository<StampsAccountEntity>
    {
        /// <summary>
        /// Returns a list of USPS (Stamps.com Expedited) accounts.
        /// </summary>
        public override IEnumerable<StampsAccountEntity> Accounts
        {
            get
            {
                return StampsAccountManager.StampsExpeditedAccounts.ToList();
            }
        }

        /// <summary>
        /// Returns a USPS (Stamps.com Expedited) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public override StampsAccountEntity GetAccount(long accountID)
        {
            return Accounts.ToList().FirstOrDefault(a => a.StampsAccountID == accountID);
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        /// <value>
        /// The default profile account.
        /// </value>
        public override StampsAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = new StampsShipmentType().GetPrimaryProfile().Postal.Stamps.StampsAccountID;
                return GetProfileAccount(ShipmentTypeCode.Usps, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(StampsAccountEntity account)
        {
            StampsAccountManager.SaveAccount(account);
        }
    }
}
