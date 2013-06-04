using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.ScanForms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps
{
    /// <summary>
    /// A Stamps.com implementation of the ScanFormCarrierAccount class.
    /// </summary>
    public class StampsScanFormAccountRepository : IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        public IEnumerable<IScanFormCarrierAccount> GetAccounts()
        {
            List<IScanFormCarrierAccount> carrierAccounts = new List<IScanFormCarrierAccount>();

            List<StampsAccountEntity> stampsAccountEntities = StampsAccountManager.Accounts;
            foreach (StampsAccountEntity accountEntity in stampsAccountEntities)
            {
                StampsScanFormCarrierAccount carrierAccount = new StampsScanFormCarrierAccount(new StampsScanFormRepository(), accountEntity);
                carrierAccounts.Add(carrierAccount);
            }

            return carrierAccounts;
        }
    }
}
