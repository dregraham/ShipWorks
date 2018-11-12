using System.Collections.Generic;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Interface for retrieving default lookup field layouts
    /// </summary>
    public interface IOrderLookupFieldLayoutDefaults
    {
        /// <summary>
        /// Return the default layouts.
        /// </summary>
        IEnumerable<SectionLayout> GetDefaults();
    }
}