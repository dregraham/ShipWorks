using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Provides online status codes for BigCommerce
    /// </summary>
    public interface IBigCommerceStatusCodeProvider
    {
        /// <summary>
        /// The list of all code names
        /// </summary>
        ICollection<string> CodeNames { get; }

        /// <summary>
        /// Update status codes from the online store
        /// </summary>
        void UpdateFromOnlineStore();

        /// <summary>
        /// Get the code value for the given name
        /// </summary>
        int GetCodeValue(string codeName);
    }
}