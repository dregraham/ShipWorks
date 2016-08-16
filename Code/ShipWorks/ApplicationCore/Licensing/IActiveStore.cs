using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for the active store response
    /// </summary>
    public interface IActiveStore
    {
        /// <summary>
        /// The store name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The store license key
        /// </summary>
        string StoreLicenseKey { get; set; }

        /// <summary>
        /// The store type code
        /// </summary>
        StoreTypeCode StoreType { get; }
    }
}