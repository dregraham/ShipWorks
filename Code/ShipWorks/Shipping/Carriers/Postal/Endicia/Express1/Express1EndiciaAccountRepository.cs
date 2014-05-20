﻿using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Endicia.Express1
{
    /// <summary>
    /// Account repository for Express1 Endicia accounts
    /// </summary>
    public class Express1EndiciaAccountRepository : CarrierAccountRepositoryBase<EndiciaAccountEntity>
    {
        /// <summary>
        /// Gets all the Express1 Endicia accounts in the system
        /// </summary>
        public override IEnumerable<EndiciaAccountEntity> Accounts
        {
            get
            {
                return EndiciaAccountManager.Express1Accounts;
            }
        }

        /// <summary>
        /// Gets the Endicia account with the specified id.
        /// </summary>
        /// <param name="accountID">Id of the account to retrieve</param>
        public override EndiciaAccountEntity GetAccount(long accountID)
        {
            EndiciaAccountEntity endiciaAccountEntity = EndiciaAccountManager.GetAccount(accountID);

			// Return null if found account is not Express1
            return (endiciaAccountEntity != null && endiciaAccountEntity.EndiciaReseller == (int) EndiciaReseller.None) ? endiciaAccountEntity : null;
        }

        /// <summary>
        /// Gets the account associated withe the default profile. A null value is returned
        /// if there is not an account associated with the default profile.
        /// </summary>
        /// <exception cref="ShippingException">An account that no longer exists is associated with the default Express1 profile.</exception>
        public override EndiciaAccountEntity DefaultProfileAccount
        {
            get
            {
                long? accountID = new Express1EndiciaShipmentType().GetPrimaryProfile().Postal.Endicia.EndiciaAccountID;
                return GetProfileAccount(ShipmentTypeCode.Express1Endicia, accountID);
            }
        }
    }
}
