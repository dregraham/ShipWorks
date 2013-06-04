using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.UPS.OpenAccount
{
    /// <summary>
    /// An implementation of the IUpsAccountRepository that uses the UpsAccountManager to
    /// persist UPS account data to the ShipWorks database.
    /// </summary>
    public class UpsAccountRepository : IUpsAccountRepository
    {
        /// <summary>
        /// Saves the given UPS account entity to the underlying data source.
        /// </summary>
        /// <param name="account">The account.</param>
        public void Save(UpsAccountEntity account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            UpsAccountManager.SaveAccount(account);
        }
    }
}
