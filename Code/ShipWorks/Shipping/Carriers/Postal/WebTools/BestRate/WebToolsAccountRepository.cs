using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.Custom.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools.BestRate
{
    /// <summary>
    /// Basic repository for retrieving postal (w/o postage) accounts
    /// </summary>
    public class WebToolsAccountRepository : ICarrierAccountRepository<NullEntity>
    {
        /// <summary>
        /// Returns a list of postal (w/o postage) accounts.
        /// </summary>
        public IEnumerable<NullEntity> Accounts
        {
            get
            {
                return new List<NullEntity> { new NullEntity() };
            }
        }

        /// <summary>
        /// Returns a postal (w/o postage) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>Null entity.</returns>
        public NullEntity GetAccount(long accountID)
        {
            return new NullEntity();
        }

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public NullEntity DefaultProfileAccount
        {
            get
            {
                return new NullEntity();
            }
        }
    }
}
