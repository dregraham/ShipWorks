﻿
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Wrapper for UspsAccountManager
    /// </summary>
    public class UspsAccountManagerWrapper : IUspsAccountManager
    {
        /// <summary>
        /// Initializes for current session.
        /// </summary>
        public void InitializeForCurrentSession()
        {
            UspsAccountManager.InitializeForCurrentSession();
        }

        /// <summary>
        /// Get the USPS accounts in the system.
        /// </summary>
        public List<UspsAccountEntity> GetAccounts(UspsResellerType resellerType)
        {
            return UspsAccountManager.GetAccounts(resellerType);
        }
    }
}