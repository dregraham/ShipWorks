using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc.Mapping
{
	public interface IOdbcFieldMapFactory
	{
		OdbcFieldMap CreateOrderFieldMap();

		OdbcFieldMap CreateOrderItemFieldMap();

		OdbcFieldMap CreateAddressFieldMap();

		OdbcFieldMap CreateFieldMapFrom(IEnumerable<OdbcFieldMap> maps);
	}
}
