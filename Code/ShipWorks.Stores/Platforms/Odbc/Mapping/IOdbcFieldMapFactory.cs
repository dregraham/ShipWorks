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
        IOdbcFieldMap CreateOrderFieldMap();

        /// <summary>
        /// Creates the order item field map.
        /// </summary>
        IOdbcFieldMap CreateOrderItemFieldMap(int index);

        /// <summary>
        /// Creates the address field map.
        /// </summary>
        IOdbcFieldMap CreateAddressFieldMap();

        /// <summary>
        /// Creates the shipment field map.
        /// </summary>
        /// <returns></returns>
        IOdbcFieldMap CreateShipmentFieldMap();

        /// <summary>
        /// Creates the shipto address field map.
        /// </summary>
        /// <returns></returns>
        IOdbcFieldMap CreateShiptoAddressFieldMap();

        /// <summary>
        /// Creates the field map from the list of entries
        /// </summary>
        OdbcFieldMap CreateFieldMapFrom(IEnumerable<IOdbcFieldMapEntry> entries);

        /// <summary>
        /// Gets a map with the specified number of attributes with item numbers started at the specified start number.
        /// </summary>
        IOdbcFieldMap GetAttributeRangeFieldMap(int startAttributeNumber, int numberOfAttributes, int itemIndex);

	}
}
