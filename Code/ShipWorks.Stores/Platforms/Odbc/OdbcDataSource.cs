using Interapptive.Shared.Utility;
using System;
using System.Data;
using System.Reflection;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using Interapptive.Shared.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ShipWorks.Stores.Platforms.Odbc
{
    /// <summary>
    /// OdbcDataSource
    /// </summary>
    public class OdbcDataSource : IOdbcDataSource
    {
        private readonly IShipWorksDbProviderFactory odbcProvider;
        private readonly IEncryptionProvider encryptionProvider;
        private string customConnectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSource(IShipWorksDbProviderFactory odbcProvider, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.odbcProvider = odbcProvider;
            encryptionProvider = encryptionProviderFactory.CreateOdbcEncryptionProvider();
        }

        /// <summary>
        /// Name of the data source
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; private set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; private set; }

        /// <summary>
        /// Gets or sets whether or not the data
        /// source is using a custom connection string.
        /// </summary>
        public bool IsCustom { get; private set; }

        /// <summary>
        /// Gets or sets the custom connection string.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return BuildConnectionString();
            }

            private set { customConnectionString = value; }
        }

        /// <summary>
        /// Changes the connection to use a custom connection string.
        /// </summary>
        /// <param name="connectionString">The custom connection string to use</param>
        public void ChangeConnection(string connectionString)
        {
            IsCustom = true;
            Name = "Custom...";
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Changes the connection to use a new dsn, username and password
        /// </summary>
        public void ChangeConnection(string dsn, string username, string password)
        {
            IsCustom = false;
            Name = dsn;
            Username = username;
            Password = password;
        }

        /// <summary>
        /// Gets the unencrypted connection string.
        /// </summary>
        private string BuildConnectionString()
        {
            // if we are using a custom connection string
            // then return the custom connection string
            if (IsCustom)
            {
                return customConnectionString;
            }

            // Build the connection string based on
            // the username password and dsn properties
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Name))
            {
                result.Append($"DSN={Name};");
            }

            if (!string.IsNullOrWhiteSpace(Username))
            {
                result.Append($"Uid={Username};");
            }

            if (!string.IsNullOrWhiteSpace(Password))
            {
                result.Append($"Pwd={Password};");
            }

            return result.ToString();
        }

        /// <summary>
        /// Tests the connection.
        /// </summary>
        /// <returns>
        /// GenericResult where Success is true if the connection is successful
        /// If there is an error, message will return the error message returned
        /// from the ODBC Driver.
        /// </returns>
        public GenericResult<IOdbcDataSource> TestConnection()
        {
            GenericResult<IOdbcDataSource> testResult= new GenericResult<IOdbcDataSource>(this);

            using (IDbConnection connection = odbcProvider.CreateOdbcConnection(BuildConnectionString()))
            {
                try
                {
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

        /// <summary>
        /// Serialize and encrypt the OdbcDataSource
        /// </summary>
        public string Serialize()
        {
            return encryptionProvider.Encrypt(JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Populate the OdbcDataSource using the given json string
        /// </summary>
        public void Restore(string json)
        {
            try
            {
                JObject dataSource = JObject.Parse(encryptionProvider.Decrypt(json));

                Name = dataSource["Name"].ToString();
                bool custom;
                IsCustom = bool.TryParse(dataSource["IsCustom"].ToString(), out custom) && custom;
                Username = dataSource["Username"].ToString();
                Password = dataSource["Password"].ToString();
                ConnectionString = dataSource["ConnectionString"].ToString();
            }
            catch (JsonReaderException)
            {
                throw new ShipWorksOdbcException("Failed to restore data source");
            }
        }

        /// <summary>
        /// Creates a new ODBC Connection
        /// </summary>
        public DbConnection CreateConnection()
        {
            return odbcProvider.CreateOdbcConnection(ConnectionString);
        }
    }
}