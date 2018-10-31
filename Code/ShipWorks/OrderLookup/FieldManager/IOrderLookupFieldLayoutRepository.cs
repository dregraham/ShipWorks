using System.Collections.Generic;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Interface for retrieving and saving order lookup field layouts
    /// </summary>
    public interface IOrderLookupFieldLayoutRepository : IOrderLookupFieldLayoutProvider
    {
        /// <summary>
        /// Save the given layouts to the database.
        /// </summary>
        void Save(IEnumerable<SectionLayout> sectionLayouts);
    }
}
