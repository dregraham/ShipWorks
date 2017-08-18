using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Provides online status codes for BigCommerce
    /// </summary>
    public interface IBigCommerceStatusCodeProvider
    {
        /// <summary>
        /// Indexer for a specific code
        /// </summary>
        string this[int code] { get; }

        /// <summary>
        /// The list of all code names
        /// </summary>
        ICollection<string> CodeNames { get; }

        /// <summary>
        /// The list of all code values
        /// </summary>
        ICollection<int> CodeValues { get; }

        /// <summary>
        /// Update status codes from the online store
        /// </summary>
        void UpdateFromOnlineStore();

        /// <summary>
        /// Get the code value for the given name
        /// </summary>
        int GetCodeValue(string codeName);

        /// <summary>
        /// Get the display text for the code
        /// </summary>
        string GetCodeName(int statusCode);
    }
}