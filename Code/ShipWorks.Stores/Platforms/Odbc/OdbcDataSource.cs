using Interapptive.Shared.Utility;
using System;
using System.Data;
using System.Text;
using Interapptive.Shared.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public OdbcDataSource(IShipWorksDbProviderFactory odbcProvider, IEncryptionProviderFactory encryptionProviderFactory)
        {
            this.odbcProvider = odbcProvider;
            encryptionProvider = encryptionProviderFactory.CreateOdbcEncryptionProvider();
        }

        /// <summary>
        /// Name to display to users
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (IsCustom)
                {
                    return "Custom...";
                }

                return Dsn;
            }
        }

        /// <summary>
        /// Name of the data source
        /// </summary>
        public string Dsn { get; private set; }

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
        public string CustomConnectionString { get; private set; }

        /// <summary>
        /// Changes the connection to use a custom connection string.
        /// </summary>
        /// <param name="connectionString">The custom connection string to use</param>
        public void ChangeConnection(string connectionString)
        {
            IsCustom = true;
            CustomConnectionString = connectionString;
        }

        /// <summary>
        /// Changes the connection to use a new dsn, username and password
        /// </summary>
        public void ChangeConnection(string dsn, string username, string password)
        {
            IsCustom = false;
            Dsn = dsn;
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
                return CustomConnectionString;
            }

            // Build the connection string based on
            // the username password and dsn properties
            StringBuilder result = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Dsn))
            {
                result.Append($"DSN={Dsn};");
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

        /// <summary>
        /// Serialize and encrypt the OdbcDataSource
        /// </summary>
        public string Seralize()
        {
            return encryptionProvider.Encrypt(JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Populate the OdbcDataSource using the given json string
        /// </summary>
        public void Populate(string json)
        {
            JObject dataSource = JObject.Parse(encryptionProvider.Decrypt(json));

            Dsn = dataSource["Name"].ToString();
            Username = dataSource["Username"].ToString();
            Password = dataSource["Password"].ToString();
            CustomConnectionString = dataSource["CustomConnectionString"].ToString();
        }
    }
}