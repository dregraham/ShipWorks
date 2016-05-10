using System;
using System.Collections.Generic;

namespace ShipWorks.Stores.Platforms.Odbc
{
	public class OdbcTable
	{
		public OdbcTable(OdbcSchema schema, string tableName)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<OdbcColumn> Columns
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public string Name
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public void Load()
		{
			throw new NotImplementedException();
		}
	}
}
