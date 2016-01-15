using System.Collections.Generic;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia
{
    /// <summary>
    /// Basic repository for retrieving Endicia accounts
    /// </summary>
    public class EndiciaAccountRepository : CarrierAccountRepositoryBase<EndiciaAccountEntity>, ICarrierAccountRepository<EndiciaAccountEntity>
    {
        /// <summary>
        /// Returns a list of Endicia accounts.
        /// </summary>
        public override IEnumerable<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.EndiciaAccounts.ToList();
            }
        }

        /// <summary>
        /// Returns a Endicia account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>The matching account.</returns>
        public override EndiciaAccountEntity GetAccount(long accountID)
        {
            EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(accountID);

            // return null if found account is not Endicia
            return endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.None ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default FedEx profile.</exception>
        public override EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = GetPrimaryProfile(ShipmentTypeCode.Endicia).Postal.Endicia.EndiciaAccountID;
                return GetProfileAccount(ShipmentTypeCode.Endicia, accountID);
            }
        }

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public override void Save(EndiciaAccountEntity account)
        {
            EndiciaAccountManager.SaveAccount(account);
        }
    }
}
