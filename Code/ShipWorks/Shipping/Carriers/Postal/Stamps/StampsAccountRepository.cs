using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// Basic repository for retrieving Stamps accounts
    /// </summary>
    public class StampsAccountRepository : CarrierAccountRepositoryBase<StampsAccountEntity>, ICarrierAccountRepository<StampsAccountEntity>
    {
        /// <summary>
        /// Returns a list of Stamps accounts.
        /// </summary>
        public override IEnumerable<StampsAccountEntity> Accounts
        {
            get
            {
                return StampsAccountManager.StampsAccounts.ToList();
            }
        }

        /// <summary>
        /// Returns a Stamps account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
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
                long? accountID = new StampsShipmentType().GetPrimaryProfile().Postal.Stamps.StampsAccountID;

                return GetProfileAccount(ShipmentTypeCode.Stamps, accountID);
            }
        }
    }
}
