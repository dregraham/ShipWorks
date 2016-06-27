#region

using System;
using System.Data;

#endregion

namespace ShipWorks.Stores.Platforms.Odbc
{
    public class OdbcSampleDataCommand : IOdbcSampleDataCommand
    {
        private readonly IShipWorksDbProviderFactory dbProviderFactory;

        private OdbcSampleDataCommand(IShipWorksDbProviderFactory dbProviderFactory)
        {
            this.dbProviderFactory = dbProviderFactory;
        }


        public DataTable Execute(IOdbcDataSource dataSource, string query)
        {
            throw new NotImplementedException();
        }
    }
}