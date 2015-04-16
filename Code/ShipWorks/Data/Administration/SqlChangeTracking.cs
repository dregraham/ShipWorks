using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Text;
using Interapptive.Shared.Data;
using log4net;
using ShipWorks.Data.Connection;

namespace ShipWorks.Data.Administration
{
    /// <summary>
    /// Provides an entry point for managing/enabling change tracking of the current ShipWorks database.
    /// </summary>
    public class SqlChangeTracking
    {
        private readonly ILog log;
        private readonly List<string> tablesRequiringChangeTracking;        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlChangeTracking"/> class.
        /// </summary>
        public SqlChangeTracking()
            : this(LogManager.GetLogger(typeof(SqlChangeTracking)))
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlChangeTracking" /> class.
        /// </summary>
        /// <param name="log">An instance of ILog that should be used for logging purposes.</param>
        public SqlChangeTracking(ILog log)
        {
            this.log = log;

            tablesRequiringChangeTracking = new List<string>
            {
                "UspsAccount",
                "UpsAccount",
                "StatusPreset",
                "TemplateFolder",
                "Template",
                "Action",
                "Computer",
                "ActionTask",
                "OrderItemAttribute",
                "Order",
                "OrderItem",
                "ActionQueue",
                "Store",
                "Note",
                "Audit",
                "User",
                "DimensionsProfile",
                "Customer",
                "EmailAccount",
                "OrderCharge",
                "ShippingProfile",
                "EndiciaAccount",
                "FedExAccount",
                "Shipment",
                "OrderPaymentDetail",
                "FtpAccount",
                "PrintResult",
                "iParcelAccount",
                "LabelSheet",
                "Download",
                "OnTracAccount",
                "EmailOutbound",
                "ServiceStatus",
                "ShippingDefaultsRule",
                "ShipmentCustomsItem",
                "ShippingOrigin",
                "ShippingPrintOutput",
                "ShippingProviderRule"
            };
        }

        /// <summary>
        /// Gets the tables requiring change tracking.
        /// </summary>
        public ReadOnlyCollection<string> TablesRequiringChangeTracking
        {
            get
            {
                return new ReadOnlyCollection<string>(tablesRequiringChangeTracking);
            }
        }

        /// <summary>
        /// Enables change tracking on the database being used in the current SqlSession along with any
        /// tables that need change tracking on an as needed basis.
        /// </summary>
        public void Enable()
        {
            if (IsDatabaseChangeTrackingDisabled())
            {
                EnableDatabaseChangeTracking();
            }

            EnableChangeTrackingOnTables();
        }

        /// <summary>
        /// Determines whether change tracking is disabled on the database being used in the current SqlSession.
        /// </summary>
        /// <returns><c>true</c> change tracking is disabled; otherwise, <c>false</c>.</returns>
        private bool IsDatabaseChangeTrackingDisabled()
        {
            const string changeTrackingQueryFormat =
                    @"SELECT COUNT(0)
                    FROM SYS.CHANGE_TRACKING_DATABASES 
                    WHERE database_id = DB_ID('{0}')";

            try
            {
                bool isDisabled = false;
                log.InfoFormat("Checking whether change tracking is enabled for database {0}.", SqlSession.Current.Configuration.DatabaseName);
                
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = string.Format(changeTrackingQueryFormat, cmd.Connection.Database);
                        int count = (int)cmd.ExecuteScalar();

                        // Consider change tracking disabled if the current database was not in the list of 
                        // change tracking databases in SQL Server
                       isDisabled = count == 0;
                    }
                }

                log.InfoFormat("Change tracking is {0} enabled on database {1}.", isDisabled ? "not" : string.Empty, SqlSession.Current.Configuration.DatabaseName);
                return isDisabled;
            }
            catch (SqlException ex)
            {
                // Log the error and assume that change tracking is enabled, so ShipWorks continues 
                log.Error(string.Format("An error occurred checking the status of change tracking for database '{0}'. ", SqlSession.Current.Configuration.DatabaseName), ex);
                return false;
            }
        }

        /// <summary>
        /// Enables change tracking at the database level.
        /// </summary>
        private void EnableDatabaseChangeTracking()
        {
            try
            {
                log.InfoFormat("Change tracking is being enabled for database {0}.", SqlSession.Current.Configuration.DatabaseName);
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = string.Format("ALTER DATABASE [{0}] SET CHANGE_TRACKING = ON", cmd.Connection.Database);
                        cmd.ExecuteNonQuery();

                        log.InfoFormat("Change tracking has been enabled for database {0}.", cmd.Connection.Database);
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error(string.Format("Error enabling change tracking for database '{0}'. ", SqlSession.Current.Configuration.DatabaseName), ex);
            }
        }

        /// <summary>
        /// Enables the change tracking on an as needed basis for all the tables specified by TablesRequiringChangeTracking.
        /// </summary>
        private void EnableChangeTrackingOnTables()
        {
            // SQL for enabling change tracking on a table if it is not already on
            const string enableTableChangeTrackingFormat =
                @"IF NOT EXISTS
                (
	                SELECT SYS.TABLES.NAME 
	
	                FROM sys.change_tracking_tables

	                INNER JOIN sys.tables 
		                ON sys.tables.object_id = sys.change_tracking_tables.object_id
	                INNER JOIN sys.schemas 
		                ON sys.schemas.schema_id = sys.tables.schema_id
	
	                WHERE sys.tables.name = '{0}'
                )
                BEGIN
                    PRINT 'Enabling change tracking on table {0}'
                    ALTER TABLE [{0}] ENABLE CHANGE_TRACKING
                END";
            
            // Build up the SQL for enabling change tracking on all the tables that require change tracking
            StringBuilder query = new StringBuilder();
            foreach (string table in TablesRequiringChangeTracking)
            {
                query.AppendFormat(enableTableChangeTrackingFormat, table);
            }

            try
            {
                log.InfoFormat("Enabling change tracking on {0} ShipWorks tables.", TablesRequiringChangeTracking.Count);
                using (SqlConnection con = SqlSession.Current.OpenConnection())
                {
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        cmd.CommandText = query.ToString();
                        cmd.ExecuteNonQuery();

                        log.InfoFormat("Change tracking has been enabled on {0} ShipWorks tables.", TablesRequiringChangeTracking.Count);
                    }
                }
            }
            catch (SqlException ex)
            {
                log.Error("An error occurred while enabling change tracking on ShipWorks tables.", ex);
            }
        }
    }
}
