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
        OdbcFieldMap CreateOrderItemFieldMap();

        /// <summary>
        /// Creates the address field map.
        /// </summary>
        OdbcFieldMap CreateAddressFieldMap();

        /// <summary>
        /// Creates the field map from.
        /// </summary>
        OdbcFieldMap CreateFieldMapFrom(IEnumerable<OdbcFieldMap> maps);
	}
}
