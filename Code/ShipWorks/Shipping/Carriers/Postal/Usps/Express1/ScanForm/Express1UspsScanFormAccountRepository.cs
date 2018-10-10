﻿using System.Collections.Generic;
using System.Linq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Postal.Usps.ScanForm;
using ShipWorks.Shipping.ScanForms;

namespace ShipWorks.Shipping.Carriers.Postal.Usps.Express1.ScanForm
{
    /// <summary>
    /// An Express1/USPS implementation of the IScanFormAccountRepository interface.
    /// </summary>
    public class Express1UspsScanFormAccountRepository : UspsScanFormAccountRepository
    {
        /// <summary>
        /// Gets the accounts that should have scan forms created for them
        /// </summary>
        protected override IEnumerable<UspsAccountEntity> AccountList
        {
            get { return UspsAccountManager.Express1Accounts; }
        }

        /// <summary>
        /// Creates a ScanFormCarrierAccount from the account entity
        /// </summary>
        /// <param name="accountEntity">Account entity for which to create the scan form carrier account</param>
        /// <returns>A new instance of IScanFormCarrierAccount</returns>
        protected override IScanFormCarrierAccount CreateScanFormCarrierAccount(UspsAccountEntity accountEntity)
        {
            return new Express1UspsScanFormCarrierAccount(new UspsScanFormRepository((UspsResellerType) accountEntity.UspsReseller), accountEntity);
        }

        /// <summary>
        /// Does the repository have any accounts
        /// </summary>
        public bool HasAccounts => UspsAccountManager.Express1AccountsReadOnly.Any(x => x.PendingInitialAccount == (int) UspsPendingAccountType.None);
    }
}
