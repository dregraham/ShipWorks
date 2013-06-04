using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace ShipWorks.Shipping.ScanForms
{
    public interface IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        IEnumerable<IScanFormCarrierAccount> GetAccounts();
    }
}
