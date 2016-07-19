using System.Data.Common;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using ShipWorks.Stores.Platforms.Odbc.Download;

namespace ShipWorks.Stores.Platforms.Odbc.Upload
{
    /// <summary>
    /// Command to upload shipment data to ODBC
    /// </summary>
    public class OdbcUploadCommand : IOdbcUploadCommand
    {
        private readonly IOdbcDataSource dataSource;
        private readonly IShipWorksDbProviderFactory dbProviderFactory;
        private readonly IOdbcQuery uploadQuery;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcUploadCommand(IOdbcDataSource dataSource,
            IShipWorksDbProviderFactory dbProviderFactory,
            IOdbcQuery uploadQuery)
        {
            this.dataSource = dataSource;
            this.dbProviderFactory = dbProviderFactory;
            this.uploadQuery = uploadQuery;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public int Execute()
        {
            try
            {
                using (DbConnection connection = dataSource.CreateConnection())
                {
                    connection.Open();

                    using (IShipWorksOdbcCommand command = dbProviderFactory.CreateOdbcCommand(connection))
                    {
                        uploadQuery.ConfigureCommand(command);
                        return command.ExecuteNonQuery();
                    }
                }
            }
            catch (DbException ex)
            {
                // cant unit test OdbcException, rethrow as ShipWorksOdbcException
                throw new ShipWorksOdbcException(ex.Message, ex);
            }
        }
    }
}