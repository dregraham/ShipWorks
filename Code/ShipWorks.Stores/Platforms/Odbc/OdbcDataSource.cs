using Interapptive.Shared.Utility;
using System;
using System.Data;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// OdbcDataSource
    /// </summary>
    public class OdbcDataSource
    {
        private readonly IShipWorksDbProviderFactory odbcProvider;
        private readonly IEncryptionProvider encryptionProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSource(IShipWorksDbProviderFactory odbcProvider, IEncryptionProvider encryptionProvider)
        {
            this.odbcProvider = odbcProvider;
            this.encryptionProvider = encryptionProvider;
        }

        /// <summary>
        /// Name of the data source
        /// </summary>
        [Obfuscation(Exclude = true)]
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
        private string BuildConnectionString()
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

        /// <summary>
        /// Gets the encrypted connection string.
        /// </summary>
        /// <remarks>
        /// Connection string is encrypted due to the possibility of containing a password.
        /// </remarks>
        public string ConnectionString =>
            encryptionProvider.Encrypt(BuildConnectionString());

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
                    connection.ConnectionString = BuildConnectionString();
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