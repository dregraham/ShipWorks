using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using System.Data;
using log4net;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Detailed information about a single ShipWorks database
    /// </summary>
    public class SqlDatabaseDetail
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlDatabaseDetail));

        string name;
        SqlDatabaseStatus status;

        Version schemaVersion;

        string lastUsedBy;
        DateTime lastUsedOn;

        string lastOrderNumber;
        DateTime lastOrderDate;

        /// <summary>
        /// Load detailed database information about the given database
        /// </summary>
        public static SqlDatabaseDetail Load(string database, SqlConnection con)
        {
            SqlDatabaseDetail detail = new SqlDatabaseDetail();
            detail.name = database;

            try
            {
                con.ChangeDatabase(database);

                bool isShipWorksDb = (int) SqlCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('GetSchemaVersion'), 0)") > 0;
                if (!isShipWorksDb)
                {
                    detail.status = SqlDatabaseStatus.NonShipWorks;
                }
                else
                {
                    LoadSchemaVersion(detail, con);
                    LoadLastUsedBy(detail, con);
                    LoadLastOrderNumber(detail, con);
                }
            }
            catch (SqlException ex)
            {
                log.Error("Could not load database detail for " + database, ex);

                detail.status = SqlDatabaseStatus.NoAccess;
            }
            catch (ArgumentException ex)
            {
                // Catching this exception to handle bad schema versions
                log.Error("Could not load database detail for " + database, ex);

                detail.status = SqlDatabaseStatus.NoAccess;
            }

            return detail;
        }

        /// <summary>
        /// Load the schema version of the ShipWorks database associated with the given connection
        /// </summary>
        private static void LoadSchemaVersion(SqlDatabaseDetail detail, SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            detail.schemaVersion = new Version((string) SqlCommandProvider.ExecuteScalar(cmd));

            detail.status = detail.schemaVersion.Major < 3 ? SqlDatabaseStatus.ShipWorks2x : SqlDatabaseStatus.ShipWorks;
        }

        /// <summary>
        /// Load the last user to use the given ShipWorsk database
        /// </summary>
        private static void LoadLastUsedBy(SqlDatabaseDetail detail, SqlConnection con)
        {
            detail.lastUsedBy = "";
            detail.lastUsedOn = DateTime.MinValue;

            // We can only load this if the Audit table exists (it wont for 2.x databases)
            if ((int) SqlCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('Audit'), 0)") > 0)
            {
                SqlCommand cmd = SqlCommandProvider.Create(con);
                cmd.CommandText =
                    "SELECT TOP (1) u.Username, a.Date " +
                    "  FROM Audit a INNER JOIN [User] u ON a.UserID = u.UserID " +
                    "  WHERE a.UserID != 1027309002 " +
                    "  ORDER BY a.AuditID DESC";

                using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                {
                    if (reader.Read())
                    {
                        detail.lastUsedBy = reader.GetString(0);
                        detail.lastUsedOn = reader.GetDateTime(1);
                    }
                }
            }
        }

        /// <summary>
        /// Load the last order found in the given ShipWorks database
        /// </summary>
        private static void LoadLastOrderNumber(SqlDatabaseDetail detail, SqlConnection con)
        {
            SqlCommand cmd = SqlCommandProvider.Create(con);

            // 2x and 3x store it differently
            if ((int) SqlCommandProvider.ExecuteScalar(con, "SELECT COALESCE(OBJECT_ID('[Order]'), 0)") > 0)
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

            using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    detail.lastOrderNumber = reader.GetString(0);
                    detail.lastOrderDate = reader.GetDateTime(1);
                }
            }
        }

        /// <summary>
        /// The name of the database
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// The status of the database, as it related to ShipWorks
        /// </summary>
        public SqlDatabaseStatus Status
        {
            get { return status; }
        }

        /// <summary>
        /// ShipWorks schema version of the database
        /// </summary>
        public Version SchemaVersion
        {
            get { return schemaVersion; }
        }

        /// <summary>
        /// The last ShipWorks user to log in to the database
        /// </summary>
        public string LastUsedBy
        {
            get { return lastUsedBy; }
        }

        /// <summary>
        /// The date\time the last ShipWorks user logged in to the database
        /// </summary>
        public DateTime LastUsedOn
        {
            get { return lastUsedOn; }
        }

        /// <summary>
        /// The last order number to be downloaded into the database
        /// </summary>
        public string LastOrderNumber
        {
            get { return lastOrderNumber; }
        }

        /// <summary>
        /// The date of the last order to be downloaded into the database
        /// </summary>
        public DateTime LastOrderDate
        {
            get { return lastOrderDate; }
        }
    }
}
