using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Licensing
{
    /// <summary>
    /// Interface for ShipWorksLicense
    /// </summary>
    public interface IShipWorksLicense
    {
        /// <summary>
        /// Gets the store type code.
        /// </summary>
        StoreTypeCode StoreTypeCode { get; }
    }
}