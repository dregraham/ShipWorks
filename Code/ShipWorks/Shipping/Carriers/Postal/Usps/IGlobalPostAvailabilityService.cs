using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.Usps
{
    /// <summary>
    /// Interface keeping track of global post services available to ShipWorks
    /// </summary>
    public interface IGlobalPostAvailabilityService
    {
        /// <summary>
        /// List of services available
        /// </summary>
        IEnumerable<PostalServiceType> Services { get; }

        /// <summary>
        /// Initialize the list of services for the current session
        /// </summary>
        void InitializeForCurrentSession();

        /// <summary>
        /// Refresh the list of services based on all accounts
        /// </summary>
        void Refresh();

        /// <summary>
        /// Refresh the list of services based on the given account
        /// </summary>
        void Refresh(UspsAccountEntity account);
    }
}