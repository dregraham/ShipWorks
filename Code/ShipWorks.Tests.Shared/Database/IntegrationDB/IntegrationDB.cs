using System;
using System.Data.SqlClient;
using System.Diagnostics;
using Interapptive.Shared.Data;

namespace ShipWorks.Tests.Shared.Database.IntegrationDB
{
    /// <summary>
    /// Database to use for integration tests
    /// </summary>
    public class IntegrationDB
    {
        /// <summary>
        /// The name of the database represented by this instance of <see cref="LocalDb"/>.
        /// </summary>
        private readonly string databaseName;
        private readonly string dataSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="IntegrationDB"/> class.
        /// </summary>
        public IntegrationDB(string databaseName, string dataSource = @"localhost")
        {
            this.databaseName = databaseName;
            this.dataSource = dataSource;
        }

        /// <summary>
        /// Gets the connection string to the SQL LocalDB database.
        /// </summary>
        public virtual string ConnectionString
        {
            get
            {
                return BuildConnectionString();
            }
        }

        /// <summary>
        /// Builds a connection string for the current database, or if a null or empty string is passed in, builds a connection string for the master database.
        /// </summary>
        /// <returns>A connection string</returns>
        protected virtual string BuildConnectionString()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = dataSource;
            sb.InitialCatalog = databaseName;
            sb.IntegratedSecurity = false;
            sb.UserID = "sa";
            sb.Password = "MyPass@word";

            return sb.ConnectionString;
        }

        /// <summary>
        /// Builds a connection string for the current database, or if a null or empty string is passed in, builds a connection string for the master database.
        /// </summary>
        protected virtual string BuildMasterConnectionString()
        {
            SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder();
            sb.DataSource = dataSource;
            sb.InitialCatalog = "master";
            sb.IntegratedSecurity = false;
            sb.UserID = "sa";
            sb.Password = "MyPass@word";
            sb.Pooling = false;

            return sb.ConnectionString;
        }

        /// <summary>
        /// Opens a connection to the SQL LocalDB database.
        /// </summary>
        /// <returns>An open connection to the SQL LocalDB database.</returns>
        public virtual SqlConnection Open()
        {
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            conn.Open();

            return conn;
        }

        /// <summary>
        /// Opens a connection to the SQL LocalDB master database.
        /// </summary>
        /// <returns>An open connection to the SQL LocalDB master database.</returns>
        public virtual SqlConnection OpenMaster()
        {
            SqlConnection conn = new SqlConnection(BuildMasterConnectionString());
            conn.Open();

            return conn;
        }

        /// <summary>
        /// Checks whether the SQL LocalDB database exists or not.
        /// </summary>
        /// <returns>True if the database exists, otherwise false.</returns>
        public virtual bool CheckExists()
        {
            using (var conn = this.OpenMaster())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    string.Format(
                        @"
IF EXISTS(SELECT * FROM sys.databases WHERE name='{0}')
BEGIN
    SELECT 1
END
ELSE
BEGIN
    SELECT 0
END",
                        this.databaseName);

                int result = (int) cmd.ExecuteScalar();
                bool exists = result == 1;

                return exists;
            }
        }

        /// <summary>
        /// Executes a create database command to create the SQL LocalDB database.
        /// </summary>
        public virtual void CreateDatabase()
        {
            var sw = Stopwatch.StartNew();

            using (var conn = this.OpenMaster())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = string.Format(
                        @"
DECLARE @FILENAME as varchar(255)
DECLARE @LOGFILENAME as varchar(255)
SET @FILENAME =    CONVERT(VARCHAR(255), SERVERPROPERTY('instancedefaultdatapath')) + '{0}.mdf';
SET @LOGFILENAME = CONVERT(VARCHAR(255), SERVERPROPERTY('instancedefaultdatapath')) + '{0}.log';
EXEC ('CREATE DATABASE [{0}]
        ON PRIMARY (NAME = [{0}], FILENAME = ''' + @FILENAME + ''' )
        LOG ON (NAME = [{0}_Log], FILENAME = ''' + @LOGFILENAME + ''' )
            ')",
                        this.databaseName);

                    cmd.ExecuteNonQuery();
                }

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
DECLARE @productVersionText nvarchar(100) = CONVERT(NVARCHAR(100), SERVERPROPERTY ('productversion'));
DECLARE @productVersion int = CONVERT(INT, LEFT(@productVersionText, CHARINDEX('.', @productVersionText) - 1));

IF @productVersion >= 14 
BEGIN
	EXEC sp_configure 'show advanced options', 1
	RECONFIGURE;
	EXEC sp_configure 'clr strict security', 0;
	RECONFIGURE;
END;
";
                    cmd.ExecuteNonQuery();
                }
            }


            sw.Stop();
            Console.WriteLine($"CreateDatabase, {databaseName.PadRight(50)}, completed in {sw.ElapsedMilliseconds} ms.");
        }

        /// <summary>
        /// Executes a drop database command to delete the SQL LocalDB database.
        /// </summary>
        public virtual void DeleteDatabase()
        {
            var sw = Stopwatch.StartNew();

            using (var conn = this.OpenMaster())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = string.Format(
                    @"
IF EXISTS(SELECT * FROM sys.databases WHERE name='{0}')
BEGIN
    ALTER DATABASE [{0}]
    SET SINGLE_USER
    WITH ROLLBACK IMMEDIATE
    DROP DATABASE [{0}]
END",
                    this.databaseName);

                cmd.ExecuteNonQuery();
            }

            sw.Stop();
            Console.WriteLine($"DeleteDatabase, {databaseName.PadRight(50)}, completed in {sw.ElapsedMilliseconds} ms.");
        }
    }
}
