using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.IO;
using ShipWorks.Users.Security;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for creating new empty databases
    /// </summary>
    public static class SqlDatabaseCreator
    {
        // Used for loading \ executing sql
        static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Installation");

        /// <summary>
        /// Create a database with the given name.
        /// </summary>
        public static void CreateDatabase(string name, SqlConnection con)
        {
            object fileResult = SqlCommandProvider.ExecuteScalar(con, "select filename from master..sysdatabases where name = 'master'");

            if (fileResult == null || fileResult is DBNull)
            {
                throw new SqlScriptException(string.Format("The user you are connecting to SQL Server as ('{0}') does not have permissions to create a database.",
                    SqlUtility.GetUsername(con)));
            }

            // Get the filepath to master
            string masterPath = Path.GetDirectoryName((string) fileResult);

            // We need it to end in a slash
            if (!masterPath.EndsWith("\\"))
            {
                masterPath += "\\";
            }

            string scriptName = "CreateDatabase.sql";

            string createDbSql = sqlLoader.LoadScript(scriptName).Content;

            // Set the path to Program Files
            createDbSql = createDbSql.Replace("{DBNAME}", name);
            createDbSql = createDbSql.Replace("{FILEPATH}", masterPath);
            createDbSql = createDbSql.Replace("{FILENAME}", DetermineAvailableFilename(masterPath, name));

            // Create the database
            SqlUtility.ExecuteScriptSql(scriptName, createDbSql, con);
        }

        /// <summary>
        /// Determine the first available name of the file to use for the database, without extension
        /// </summary>
        private static string DetermineAvailableFilename(string masterPath, string databaseName)
        {
            // First check that just the database name is available
            if (!File.Exists(Path.Combine(masterPath, databaseName + ".mdf")) &&
                !File.Exists(Path.Combine(masterPath, databaseName + "_log.ldf")))
            {
                return databaseName;
            }

            int counter = 1;

            while (true)
            {
                if (!File.Exists(Path.Combine(masterPath, string.Format("{0}{1}.mdf", databaseName, counter))) &&
                    !File.Exists(Path.Combine(masterPath, string.Format("{0}{1}_log.ldf", databaseName, counter))))
                {
                    return databaseName + counter;
                }

                counter++;
            }
        }

        /// <summary>
        /// Creates an initial ShipWorks database schema 
        /// </summary>
        public static void CreateSchemaAndData()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                // Create the ShipWorks schema
                sqlLoader["CreateSchema"].Execute(con);

                SqlAssemblyDeployer.DeployAssemblies(con);

                // Update the database to be marked with the correct db version
                SqlSchemaUpdater.UpdateSchemaVersionStoredProcedure(con);
            }

            // Create the ShipWorks "SuperUser"
            SuperUser.Create(SqlAdapter.Default);

            // Create all the data that is needed for a fresh install of shipworks.
            InitialDataLoader.CreateCoreRequiredData();
            InitialDataLoader.CreateDefaultFreshInstallData();
            InitialDataLoader.AddSqlScriptInitialDataToDatabase();
        }

        /// <summary>
        /// Drop the database of the given name from the server instance of the given SQL Session
        /// </summary>
        public static void DropDatabase(SqlSession sqlSession, string databaseName)
        {
            using (SqlConnection con = sqlSession.OpenConnection())
            {
                con.ChangeDatabase("master");

                // This frees any existing connections so the db is not marked as in use
                SqlConnection.ClearAllPools();

                SqlCommandProvider.ExecuteNonQuery(con, "drop database " + databaseName);
            }
        }
    }
}
