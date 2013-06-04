using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.IO;

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
            createDbSql = createDbSql.Replace("{FILEPATH}", masterPath);
            createDbSql = createDbSql.Replace("{DBNAME}", name);

            // Create the database
            SqlUtility.ExecuteScriptSql(scriptName, createDbSql, con);
        }
    }
}
