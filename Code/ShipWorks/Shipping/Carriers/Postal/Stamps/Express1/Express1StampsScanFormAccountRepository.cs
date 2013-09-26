using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// An Express1/Stamps.com implementation of the IScanFormAccountRepository interface.
    /// </summary>
    public class Express1StampsScanFormAccountRepository : StampsScanFormAccountRepository
    {
        /// <summary>
        /// Gets the accounts that should have scan forms created for them
        /// </summary>
        protected override IEnumerable<StampsAccountEntity> AccountList
        {
            get { return StampsAccountManager.Express1Accounts; }
        }

        /// <summary>
        /// Creates a ScanFormCarrierAccount from the account entity
        /// </summary>
        /// <param name="accountEntity">Account entity for which to create the scan form carrier account</param>
        /// <returns>A new instance of IScanFormCarrierAccount</returns>
        protected override IScanFormCarrierAccount CreateScanFormCarrierAccount(StampsAccountEntity accountEntity)
        {
            return new Express1StampsScanFormCarrierAccount(new StampsScanFormRepository(false), accountEntity);
        }
    }
}
