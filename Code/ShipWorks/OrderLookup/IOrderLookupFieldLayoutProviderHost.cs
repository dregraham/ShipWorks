using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface for classes that implement IOrderLookupFieldLayoutProvider FieldLayoutProvider
    /// </summary>
    public interface IOrderLookupFieldLayoutProviderHost
    {
        /// <summary>
        /// Field layout repository
        /// </summary>
        IOrderLookupFieldLayoutProvider FieldLayoutProvider { get; }
    }
}