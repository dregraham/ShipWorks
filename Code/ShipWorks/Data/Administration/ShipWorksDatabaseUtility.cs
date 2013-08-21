﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Connection;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.IO;
using ShipWorks.Users.Security;
using System.Data;
using Interapptive.Shared.Win32;
using ShipWorks.ApplicationCore;
using Microsoft.Win32;
using System.Text.RegularExpressions;
using ShipWorks.Data.Administration.SqlServerSetup;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Utility class for creating new empty databases
    /// </summary>
    public static class ShipWorksDatabaseUtility
    {
        // Used for loading \ executing sql
        static SqlScriptLoader sqlLoader = new SqlScriptLoader("ShipWorks.Data.Administration.Scripts.Installation");

        // We put the underscore so we can try to not conflict with databases users may end up creating on their own
        static string localDbBaseName = "_ShipWorks";

        /// <summary>
        /// Create a database with the given name in the default SQL server data path
        /// </summary>
        public static void CreateDatabase(string name, SqlConnection con)
        {
            CreateDatabase(name, SqlUtility.GetMasterDataFilePath(con), con);
        }

        /// <summary>
        /// Create a database with the given name in the given path
        /// </summary>
        public static void CreateDatabase(string name, string path, SqlConnection con)
        {
            object result = SqlCommandProvider.ExecuteScalar(con, string.Format("SELECT name FROM master.dbo.sysdatabases WHERE name = N'{0}'", name));

            var resultString = result as string;

            if (resultString != null && resultString == name)
            {
                throw new SqlScriptException(string.Format("A database named '{0}' already exists.", name));
            }

            string createDbSql = sqlLoader.LoadScript("CreateDatabase.sql").Content;

            // We need it to end in a slash
            if (!path.EndsWith("\\"))
            {
                path += "\\";
            }

            // Set the path to Program Files
            createDbSql = createDbSql.Replace("{DBNAME}", name);
            createDbSql = createDbSql.Replace("{FILEPATH}", path);
            createDbSql = createDbSql.Replace("{FILENAME}", DetermineAvailableFileName(path, name));

            // Create the database
            SqlUtility.ExecuteScriptSql("Create Database", createDbSql, con);
        }

        /// <summary>
        /// Determine the first available name of the file to use for the database, without extension.  This checks to make sure both filename.mdf and filename_log.ldf are available
        /// </summary>
        public static string DetermineAvailableFileName(string path, string databaseName)
        {
            // First check that just the database name is available
            if (!File.Exists(Path.Combine(path, databaseName + ".mdf")) &&
                !File.Exists(Path.Combine(path, databaseName + "_log.ldf")))
            {
                return databaseName;
            }

            int counter = 1;

            while (true)
            {
                if (!File.Exists(Path.Combine(path, string.Format("{0}{1}.mdf", databaseName, counter))) &&
                    !File.Exists(Path.Combine(path, string.Format("{0}{1}_log.ldf", databaseName, counter))))
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

        /// <summary>
        /// Get the database name to use for LocalDb for this instance of ShipWorks
        /// </summary>
        public static string LocalDbDatabaseName
        {
            get
            {
                // This opens up the Current User hive, which
                // - we know we will have read\write access to
                // - is fine, b\c we are using implicit LocalDb databases, which are relative to the current user account, so we don't need LocalMachine anyway
                RegistryHelper registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\LocalDB");

                // See if we've already created a database name for this ShipWorks instance
                string databaseName = registry.GetValue(ShipWorksSession.InstanceID.ToString("B"), "");
                if (!string.IsNullOrEmpty(databaseName))
                {
                    return databaseName;
                }

                int largestIndex = -1;

                // Find out all the names that have been used so far
                foreach (string name in GetLocalDbDatabaseNames())
                {
                    int? parsed = null;

                    // Special case for the non-indexed one we store
                    if (name == localDbBaseName)
                    {
                        parsed = 0;
                    }
                    else
                    {
                        Match match = Regex.Match(name, string.Format(@"^{0}(\d)$", localDbBaseName));
                        if (match.Success)
                        {
                            parsed = int.Parse(match.Groups[1].Value);
                        }
                    }

                    if (parsed.HasValue)
                    {
                        largestIndex = Math.Max(largestIndex, parsed.Value);
                    }
                }

                // Now using the last known largest index, create the name of the database that will now be associated with this path
                databaseName = string.Format("{0}{1}", localDbBaseName, largestIndex == -1 ? "" : (largestIndex + 1).ToString());

                // Store it so this path is forever more this database name
                registry.SetValue(ShipWorksSession.InstanceID.ToString("B"), databaseName);

                return databaseName;
            }
        }

        /// <summary>
        /// Returns true if the given name is one we have recorded as a database name we've created in LocalDB
        /// </summary>
        public static bool IsLocalDbDatabaseName(string name)
        {
            return GetLocalDbDatabaseNames().Contains(name);
        }

        /// <summary>
        /// Get the names of all LocalDB databases we have recorded as having created
        /// </summary>
        private static List<string> GetLocalDbDatabaseNames()
        {
            List<string> names = new List<string>();

            RegistryHelper registry = new RegistryHelper(@"Software\Interapptive\ShipWorks\LocalDB");

            using (RegistryKey key = registry.OpenKey(null))
            {
                if (key != null)
                {
                    foreach (string name in key.GetValueNames())
                    {
                        string value = key.GetValue(name) as string;
                        if (!string.IsNullOrEmpty(value))
                        {
                            names.Add(value);
                        }
                    }
                }
            }

            return names;
        }

        /// <summary>
        /// Get the name of the database that the current session of ShipWorks should connect to in simple\automatic mode.  (The very first ShipWorks
        /// setup where the user doesn't have any choices).  It may return null if and only if SqlInstanceUtility.AutomaticServerInstance is a valid
        /// full instance, but there is no database yet created for it related to this ShipWorksSession.
        /// </summary>
        public static string AutomaticDatabaseName
        {
            get
            {
                // If the automatic server instance is local db, then we use the local db database name
                if (SqlInstanceUtility.AutomaticServerInstance == SqlInstanceUtility.LocalDbServerInstance)
                {
                    return LocalDbDatabaseName;
                }
                else
                {
                    using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\Interapptive\ShipWorks\Database"))
                    {
                        if (key != null)
                        {
                            string database = key.GetValue(ShipWorksSession.InstanceID.ToString("B")) as string;

                            // It may be null.  Null indicates to the caller that the full server instance is ready, but no database has been created yet
                            return database;
                        }
                    }

                    return null;
                }
            }
        }

        /// <summary>
        /// Get all of the deails about all of the databases on the instance of the connection
        /// </summary>
        public static List<SqlDatabaseDetail> GetDatabaseDetails(SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "select name from master..sysdatabases where name not in ('master', 'model', 'msdb', 'tempdb')";

            List<string> names = new List<string>();

            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
            {
                while (reader.Read())
                {
                    names.Add((string) reader["name"]);
                }
            }

            List<SqlDatabaseDetail> details = new List<SqlDatabaseDetail>();

            // Go through each database loading ShipWorks info about it
            foreach (string name in names)
            {
                details.Add(GetDatabaseDetail(name, con));
            }

            return details;
        }

        /// <summary>
        /// Get detailed information about the given database
        /// </summary>
        public static SqlDatabaseDetail GetDatabaseDetail(string database, SqlConnection con)
        {
            return SqlDatabaseDetail.Load(database, con);
        }

        /// <summary>
        /// Get the first available database name that doesn't conflict with any other databases on the server reprsented by the given connection
        /// </summary>
        public static string GetFirstAvailableDatabaseName(SqlConnection con)
        {
            string baseName = "ShipWorks";
            string databaseName = baseName;

            List<string> existingNames = ShipWorksDatabaseUtility.GetDatabaseDetails(con).Select(d => d.Name).ToList();

            int index = 1;
            while (existingNames.Contains(databaseName))
            {
                databaseName = string.Format("{0}{1}", baseName, index++);
            }

            return databaseName;
        }
    }
}
