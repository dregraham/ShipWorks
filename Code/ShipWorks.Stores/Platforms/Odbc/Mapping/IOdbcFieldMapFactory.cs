using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
    /// <summary>
    /// Factory for creating Odbc field maps
    /// </summary>
    public interface IOdbcFieldMapFactory
	{
        /// <summary>
        /// Creates the order field map.
        /// </summary>
        OdbcFieldMap CreateOrderFieldMap();

        /// <summary>
        /// Creates the order item field map.
        /// </summary>
        OdbcFieldMap CreateOrderItemFieldMap(int index);

        /// <summary>
        /// Creates the address field map.
        /// </summary>
        OdbcFieldMap CreateAddressFieldMap();

        /// <summary>
        /// Creates the field map from the list of entries
        /// </summary>
        OdbcFieldMap CreateFieldMapFrom(IEnumerable<IOdbcFieldMapEntry> entries);

	}
}
