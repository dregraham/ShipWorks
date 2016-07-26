using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource;
using System.Data.Common;

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
        /// Gets the name of the ODBC driver being used to execute this command.
        /// </summary>
        public string Driver
        {
            get { return dataSource.Driver; }
        }

        /// <summary>
        /// Executes the command and returns the number of rows affected.
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