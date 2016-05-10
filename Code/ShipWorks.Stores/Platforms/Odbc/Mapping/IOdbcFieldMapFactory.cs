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
        /// <returns></returns>
        OdbcFieldMap CreateOrderFieldMap();

        /// <summary>
        /// Creates the order item field map.
        /// </summary>
        /// <returns></returns>
        OdbcFieldMap CreateOrderItemFieldMap();

        /// <summary>
        /// Creates the address field map.
        /// </summary>
        /// <returns></returns>
        OdbcFieldMap CreateAddressFieldMap();

        /// <summary>
        /// Creates the field map from.
        /// </summary>
        /// <param name="maps">The maps.</param>
        /// <returns></returns>
        OdbcFieldMap CreateFieldMapFrom(IEnumerable<OdbcFieldMap> maps);
	}
}
