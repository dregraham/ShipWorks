using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.ScanForms
{
    /// <summary>
    /// Account repository that can be used for SCAN forms
    /// </summary>
    [Service]
    public interface IScanFormAccountRepository
    {
        /// <summary>
        /// Gets all of the accounts for a specific shipping carrier.
        /// </summary>
        /// <returns>A collection of the ScanFormCarrierAccount objects.</returns>
        IEnumerable<IScanFormCarrierAccount> GetAccounts();

        /// <summary>
        /// Does the repository have any accounts
        /// </summary>
        bool HasAccounts { get; }
    }
}
