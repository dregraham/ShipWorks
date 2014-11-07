using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// Account repository for Express1 Stamps accounts
    /// </summary>
    public class Express1StampsAccountRepository : CarrierAccountRepositoryBase<StampsAccountEntity>, ICarrierAccountRepository<StampsAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Stamps accounts in the system
        /// </summary>
        public override IEnumerable<StampsAccountEntity> Accounts
        {
            get
            {
                return StampsAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Gets the Stamps account with the specified id
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override StampsAccountEntity GetAccount(long accountID)
        {
            return StampsAccountManager.GetAccount(accountID);
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
                long? accountID = new Express1StampsShipmentType().GetPrimaryProfile().Postal.Stamps.StampsAccountID;
                return GetProfileAccount(ShipmentTypeCode.Express1Stamps, accountID);
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
