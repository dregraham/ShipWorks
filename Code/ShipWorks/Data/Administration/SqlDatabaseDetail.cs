using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.Data;
using log4net;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Detailed information about a single ShipWorks database
    /// </summary>
    public class SqlDatabaseDetail : ISqlDatabaseDetail
    {
        static readonly ILog log = LogManager.GetLogger(typeof(SqlDatabaseDetail));

        /// <summary>
        /// Load detailed database information about the given database
        /// </summary>
        public static async Task<ISqlDatabaseDetail> Load(string database, DbConnection con)
        {
            SqlDatabaseDetail detail = new SqlDatabaseDetail
            {
                Name = database
            };

            try
            {
                con.ChangeDatabase(database);

                var command = con.CreateCommand("SELECT COALESCE(OBJECT_ID('GetSchemaVersion'), 0)");
                bool isShipWorksDb = (int) await command.ExecuteScalarAsync().ConfigureAwait(false) > 0;
                if (!isShipWorksDb)
                {
                    detail.Status = SqlDatabaseStatus.NonShipWorks;
                }
                else
                {
                    await LoadSchemaVersion(detail, con).ConfigureAwait(false);
                    await LoadLastUsedBy(detail, con).ConfigureAwait(false);
                    await LoadLastOrderNumber(detail, con).ConfigureAwait(false);
                    await LoadArchiveDetails(detail, con).ConfigureAwait(false);
                    await LoadDatabaseGuid(detail, con).ConfigureAwait(false);
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
        private static async Task LoadDatabaseGuid(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "GetDatabaseGuid";
            cmd.CommandType = CommandType.StoredProcedure;

            detail.Guid = (Guid) await cmd.ExecuteScalarAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Load whether the database is an archive
        /// </summary>
        [SuppressMessage("Recommendations",
            "RECS0022: Empty general catch clause suppresses any error",
            Justification = "A failure when checking for whether the database is an archive should not be considered a major failure")]
        private static async Task LoadArchiveDetails(SqlDatabaseDetail detail, DbConnection con)
        {
            if (detail.SchemaVersion < ConfigurationData.ArchiveVersion)
            {
                return;
            }

            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "SELECT TOP (1) ArchivalSettingsXml FROM [Configuration]";

            try
            {
                var archivalSettingsXml = (string) await cmd.ExecuteScalarAsync().ConfigureAwait(false);
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
        private static async Task LoadSchemaVersion(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);
            cmd.CommandText = "GetSchemaVersion";
            cmd.CommandType = CommandType.StoredProcedure;

            detail.SchemaVersion = new Version((string) await cmd.ExecuteScalarAsync().ConfigureAwait(false));
            detail.Status = SqlDatabaseStatus.ShipWorks;
        }

        /// <summary>
        /// Load the last user to use the given ShipWorsk database
        /// </summary>
        private static async Task LoadLastUsedBy(SqlDatabaseDetail detail, DbConnection con)
        {
            detail.LastUsedBy = "";
            detail.LastUsedOn = DateTime.MinValue;

            // We can only load this if the Audit table exists (it wont for 2.x databases)
            if (await IsNewerThanShipWorks2(con).ConfigureAwait(false))
            {
                DbCommand cmd = DbCommandProvider.Create(con);
                cmd.CommandText =
                    "SELECT TOP (1) u.Username, a.Date " +
                    "  FROM Audit a INNER JOIN [User] u ON a.UserID = u.UserID " +
                    "  WHERE a.UserID != 1027309002 " +
                    "  ORDER BY a.AuditID DESC";

                using (DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    if (await reader.ReadAsync().ConfigureAwait(false))
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
        private static async Task LoadLastOrderNumber(SqlDatabaseDetail detail, DbConnection con)
        {
            DbCommand cmd = DbCommandProvider.Create(con);

            // 2x and 3x store it differently
            if (await IsNewerThanShipWorks2(con).ConfigureAwait(false))
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

            using (DbDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                if (await reader.ReadAsync().ConfigureAwait(false))
                {
                    detail.LastOrderNumber = reader.GetString(0);
                    detail.LastOrderDate = reader.GetDateTime(1);
                }
            }
        }

        /// <summary>
        /// Is the database ShipWorks 3 or newer
        /// </summary>
        private static async Task<bool> IsNewerThanShipWorks2(DbConnection con)
        {
            var command = con.CreateCommand();
            command.CommandText = "SELECT COALESCE(OBJECT_ID('[Order]'), 0)";
            var result = (int) await command.ExecuteScalarAsync().ConfigureAwait(false);
            return result > 0;
        }

        /// <summary>
        /// Returns the total number of orders in database
        /// </summary>
        private static async Task<int> GetOrderCount(DbConnection con)
        {
            DbCommand command = con.CreateCommand();
            command.CommandText = "SELECT COUNT(*) FROM [Order]";
            int result = (int) await command.ExecuteScalarAsync().ConfigureAwait(false);

            return result;
        }

        /// <summary>
        /// Returns the oldest order date in the database
        /// </summary>
        private static async Task<DateTime> GetOldestOrderDate(DbConnection con)
        {
            DbCommand command = con.CreateCommand();
            command.CommandText = "SELECT MIN(OrderDate) FROM [Order]";
            DateTime result = (DateTime) await command.ExecuteScalarAsync().ConfigureAwait(false);

            return result;
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
