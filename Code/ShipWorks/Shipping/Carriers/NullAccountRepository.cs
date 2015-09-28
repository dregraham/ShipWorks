using System.Collections.Generic;
using ShipWorks.Data.Model.Custom;
using ShipWorks.Data.Model.Custom.EntityClasses;

namespace ShipWorks.Shipping.Carriers
{
    /// <summary>
    /// Basic repository for retrieving postal (w/o postage) accounts
    /// </summary>
    public class NullAccountRepository : ICarrierAccountRepository<NullEntity>
    {
        /// <summary>
        /// Returns a list of postal (w/o postage) accounts.
        /// </summary>
        public IEnumerable<NullEntity> Accounts => new List<NullEntity> { new NullEntity() };

        /// <summary>
        /// Returns a postal (w/o postage) account for the provided accountID.
        /// </summary>
        /// <param name="accountID">The account ID for which to return an account.</param>
        /// <returns>Null entity.</returns>
        public NullEntity GetAccount(long accountID) => new NullEntity();

        /// <summary>
        /// Gets the default profile account.
        /// </summary>
        public NullEntity DefaultProfileAccount => new NullEntity();

        /// <summary>
        /// Saves the specified account.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(NullEntity account)
        {
            // Nothing to save
        }
    }
}
