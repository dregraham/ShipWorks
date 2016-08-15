using Interapptive.Shared.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using System;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;

namespace ShipWorks.Stores.Platforms.Odbc.DataSource
{
    /// <summary>
    /// OdbcDataSource
    /// </summary>
    [Obfuscation(Exclude=true)]
    public class OdbcDataSource : IOdbcDataSource
    {
        private readonly IShipWorksDbProviderFactory odbcProvider;
        private string customConnectionString;

        /// <summary>
        /// Constructor
        /// </summary>
        public OdbcDataSource(IShipWorksDbProviderFactory odbcProvider)
        {
            this.odbcProvider = odbcProvider;

            Name = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            Driver = string.Empty;
        }

        /// <summary>
        /// Name of the data source
        /// </summary>
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
        /// Gets the name of the ODBC driver for the data source.
        /// </summary>
        /// <value>The driver.</value>
        public string Driver { get; private set; }

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
            Driver = "Customized connection string";
        }

        /// <summary>
        /// Changes the connection to use a new dsn, username and password.
        /// </summary>
        public void ChangeConnection(string dsn, string username, string password)
        {
            ChangeConnection(dsn, username, password, "Unknown");
        }

        /// <summary>
        /// Changes the connection to use a new dsn, username, password, and ODBC driver
        /// used to connect to the data source.
        /// </summary>
        public void ChangeConnection(string dsn, string username, string password, string driver)
        {
            IsCustom = false;
            Name = dsn;
            Username = username;
            Password = password;
            Driver = string.IsNullOrWhiteSpace(driver) ? "Unknown" : driver;
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
            GenericResult<IOdbcDataSource> testResult;
            try
            {
                using (IDbConnection connection = odbcProvider.CreateOdbcConnection(BuildConnectionString()))
                {
                    connection.Open();
                    testResult = GenericResult.FromSuccess((IOdbcDataSource) this);
                }
            }
            catch (Exception ex)
            {
                testResult = GenericResult.FromError(ex.Message, (IOdbcDataSource) this);
            }
            return testResult;
        }

        /// <summary>
        /// Serialize the OdbcDataSource
        /// </summary>
        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// Populate the OdbcDataSource using the given json string
        /// </summary>
        public virtual void Restore(string json)
        {
            try
            {
                JObject dataSource = JObject.Parse(json);

                bool custom;
                IsCustom = bool.TryParse(dataSource["IsCustom"].ToString(), out custom) && custom;

                Name = dataSource["Name"].ToString();
                Username = dataSource["Username"].ToString();
                Password = dataSource["Password"].ToString();
                ConnectionString = dataSource["ConnectionString"].ToString();
                Driver = dataSource["Driver"]?.ToString() ?? "Unknown";
            }
            catch (JsonReaderException ex)
            {
                throw new ShipWorksOdbcException("Failed to restore data source", ex);
            }
        }

        /// <summary>
        /// Creates a new ODBC Connection
        /// </summary>
        /// <exception cref="ShipWorksOdbcException">The Connection string is not valid</exception>
        public DbConnection CreateConnection()
        {
            return odbcProvider.CreateOdbcConnection(ConnectionString);
        }
    }
}