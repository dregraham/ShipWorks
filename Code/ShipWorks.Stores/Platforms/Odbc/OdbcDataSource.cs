using Interapptive.Shared.Utility;
using System;
using System.Data;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// OdbcDataSource
    /// </summary>
    public class OdbcDataSource
    {
        private readonly IShipWorksOdbcProvider odbcProvider;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSource(IShipWorksOdbcProvider odbcProvider, IEncryptionProvider encryptionProvider)
        {
            this.odbcProvider = odbcProvider;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Gets or sets the DSN name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets the unencrypted connection string.
        /// </summary>
        private string UnencryptedConnectionString
        {
            get
            {
                string connectionString = string.Empty;

                if (!string.IsNullOrWhiteSpace(Name))
                {
                    connectionString = $"DSN={Name};";
                }
                if (!string.IsNullOrWhiteSpace(Username))
                {
                    connectionString += $"Uid={Username};";
                }
                if (!string.IsNullOrWhiteSpace(Password))
                {
                    connectionString += $"Pwd={Password};";
                }
                return connectionString;
            }
        }

        /// <summary>
        /// Gets the encrypted connection string.
        /// </summary>
        public string ConnectionString =>
            encryptionProvider.Encrypt(UnencryptedConnectionString);

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>
        /// GenericResult where Success is true if the connection is successful
        /// If there is an error, message will return the error message returned
        /// from the ODBC Driver.
        /// </returns>
        public GenericResult<OdbcDataSource> TestConnection()
        {
            GenericResult<OdbcDataSource> testResult= new GenericResult<OdbcDataSource>(this);

            using (IDbConnection connection = odbcProvider.CreateOdbcConnection())
            {
                try
                {
                    connection.ConnectionString = UnencryptedConnectionString;

                    connection.Open();

                    testResult.Success = true;
                }
                catch (Exception ex)
                {
                    testResult.Success = false;
                    testResult.Message = ex.Message;
                }
            }

            return testResult;
        }
    }
}