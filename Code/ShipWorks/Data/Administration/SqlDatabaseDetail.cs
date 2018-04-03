using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Xml.Linq;
using Interapptive.Shared.Data;
using log4net;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Detailed information about a single ShipWorks database
    /// </summary>
    public class SqlDatabaseDetail
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlDatabaseDetail));

        /// <summary>
        /// Load detailed database information about the given database
        /// </summary>
        public static SqlDatabaseDetail Load(string database, DbConnection con)
        {
            SqlDatabaseDetail detail = new SqlDatabaseDetail();
            detail.Name = database;

            try
            {
                con.ChangeDatabase(database);

                bool isShipWorksDb = (int) DbCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('GetSchemaVersion'), 0)") > 0;
                if (!isShipWorksDb)
                {
                    detail.Status = SqlDatabaseStatus.NonShipWorks;
                }
                else
                {
                    LoadSchemaVersion(detail, con);
                    LoadLastUsedBy(detail, con);
                    LoadLastOrderNumber(detail, con);
                    LoadArchiveDetails(detail, con);
                    LoadDatabaseGuid(detail, con);
                }
            }
            catch (SqlException ex)
            {
                log.Error("Could not load database detail for " + database, ex);

                detail.Status = SqlDatabaseStatus.NoAccess;
            }
            catch (ArgumentException ex)
            {
                // Catching this exception to handle bad schema versions
                log.Error("Could not load database detail for " + database, ex);

                detail.Status = SqlDatabaseStatus.NoAccess;
            }

            return detail;
        }

        /// <summary>
        /// Load the database GUID
        /// </summary>
        private static void LoadDatabaseGuid(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "GetDatabaseGuid";
            cmd.CommandType = CommandType.StoredProcedure;

            detail.Guid = (Guid) DbCommandProvider.ExecuteScalar(cmd);
        }

        /// <summary>
        /// Load whether the database is an archive
        /// </summary>
        private static void LoadArchiveDetails(SqlDatabaseDetail detail, DbConnection con)
        {
            if (detail.SchemaVersion < ConfigurationData.ArchiveVersion)
            {
                return;
            }

            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "SELECT TOP (1) ArchivalSettingsXml FROM [Configuration]";

            try
            {
                var archivalSettingsXml = (string) cmd.ExecuteScalar();
                detail.IsArchive = XDocument.Parse(archivalSettingsXml)?.Root?.HasElements == true;
            }
            catch (Exception)
            {
                // Do nothing, this is probably not an archive
            }
        }

        /// <summary>
        /// Load the schema version of the ShipWorks database associated with the given connection
        /// </summary>
        private static void LoadSchemaVersion(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            detail.SchemaVersion = new Version((string) DbCommandProvider.ExecuteScalar(cmd));
            detail.Status = SqlDatabaseStatus.ShipWorks;
        }

        /// <summary>
        /// Load the last user to use the given ShipWorsk database
        /// </summary>
        private static void LoadLastUsedBy(SqlDatabaseDetail detail, DbConnection con)
        {
            detail.LastUsedBy = "";
            detail.LastUsedOn = DateTime.MinValue;

            // We can only load this if the Audit table exists (it wont for 2.x databases)
            if ((int) DbCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('Audit'), 0)") > 0)
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText =
                    "SELECT TOP (1) u.Username, a.Date " +
                    "  FROM Audit a INNER JOIN [User] u ON a.UserID = u.UserID " +
                    "  WHERE a.UserID != 1027309002 " +
                    "  ORDER BY a.AuditID DESC";

                using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        detail.LastUsedBy = reader.GetString(0);
                        detail.LastUsedOn = reader.GetDateTime(1);
                    }
                }
            }
        }

        /// <summary>
        /// Load the last order found in the given ShipWorks database
        /// </summary>
        private static void LoadLastOrderNumber(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);

            // 2x and 3x store it differently
            if ((int) DbCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('[Order]'), 0)") > 0)
            {
                cmd.CommandText =
                    "SELECT TOP (1) OrderNumberComplete as OrderNumber, OrderDate " +
                    "  FROM [Order] " +
                    "  ORDER BY OrderID DESC";
            }
            else
            {
                cmd.CommandText =
                    "SELECT TOP (1) OrderNumberDisplay as OrderNumber, OrderDate " +
                    "  FROM [Orders] " +
                    "  ORDER BY OrderID DESC";
            }

            using (DbDataReader reader = DbCommandProvider.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    detail.LastOrderNumber = reader.GetString(0);
                    detail.LastOrderDate = reader.GetDateTime(1);
                }
            }
        }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The status of the database, as it related to ShipWorks
        /// </summary>
        public SqlDatabaseStatus Status { get; private set; }

        /// <summary>
        /// ShipWorks schema version of the database
        /// </summary>
        public Version SchemaVersion { get; private set; }

        /// <summary>
        /// The last ShipWorks user to log in to the database
        /// </summary>
        public string LastUsedBy { get; private set; }

        /// <summary>
        /// The date/time the last ShipWorks user logged in to the database
        /// </summary>
        public DateTime LastUsedOn { get; private set; }

        /// <summary>
        /// The last order number to be downloaded into the database
        /// </summary>
        public string LastOrderNumber { get; private set; }

        /// <summary>
        /// The date of the last order to be downloaded into the database
        /// </summary>
        public DateTime LastOrderDate { get; private set; }

        /// <summary>
        /// Is the database an archive
        /// </summary>
        public bool IsArchive { get; private set; }

        /// <summary>
        /// GUID of the database
        /// </summary>
        public Guid Guid { get; private set; }
    }
}
