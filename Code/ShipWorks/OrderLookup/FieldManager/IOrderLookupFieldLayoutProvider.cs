using System.Collections.Generic;

namespace ShipWorks.OrderLookup.FieldManager
{
    /// <summary>
    /// Interface for retrieving and saving order lookup field layouts
    /// </summary>
    public interface IOrderLookupFieldLayoutProvider
    {
        /// <summary>
        /// Fetch the section layouts from the database.
        /// </summary>
        IEnumerable<SectionLayout> Fetch();
    }
}
