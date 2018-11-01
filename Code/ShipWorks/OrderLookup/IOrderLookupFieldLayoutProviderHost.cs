using System.Reflection;
using ShipWorks.OrderLookup.FieldManager;

namespace ShipWorks.OrderLookup
{
    /// <summary>
    /// Interface for classes that implement IOrderLookupFieldLayoutProvider FieldLayoutProvider
    /// </summary>
    [Obfuscation(Exclude = true, ApplyToMembers = true, StripAfterObfuscation = false)]
    public interface IOrderLookupFieldLayoutProviderHost
    {
        /// <summary>
        /// Field layout repository
        /// </summary>
        IOrderLookupFieldLayoutProvider FieldLayoutProvider { get; }
    }
}