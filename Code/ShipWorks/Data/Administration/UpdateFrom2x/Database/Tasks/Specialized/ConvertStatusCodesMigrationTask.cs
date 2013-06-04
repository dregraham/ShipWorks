using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Interapptive.Shared.Data;
using ShipWorks.Stores;
using ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized.Utility;
using System.Transactions;
using System.Data;
using log4net;

namespace ShipWorks.Data.Administration.UpdateFrom2x.Database.Tasks.Specialized
{
    /// <summary>
    /// Converts the status code blobs on the V2 Stores table to be V3 compatible
    /// </summary>
    public class ConvertStatusCodesMigrationTask : MigrationTaskBase
    {
        // container class to hold pending work
        class CurrentConversion
        {
            public long StoreID { get; set; }
            public int StoreTypeCode { get; set; }
            public string StatusBlob { get; set; }
            public string ConvertedStatusBlob { get; set; }
        }

        // Logger 
        static readonly ILog log = LogManager.GetLogger(typeof(ConvertStatusCodesMigrationTask));
						  
        /// <summary>
        /// Gets the tyepcode for this task.
        /// </summary>
        public override MigrationTaskTypeCode TaskTypeCode
        {
            get { return MigrationTaskTypeCode.ConvertStatusCodesTask; } 
        }

        /// <summary>
        /// Copy Constructor
        /// </summary>
        public ConvertStatusCodesMigrationTask(ConvertStatusCodesMigrationTask toCopy)
            : base(toCopy)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConvertStatusCodesMigrationTask()
            : base(WellKnownMigrationTaskIds.ConvertStatusCodes, MigrationTaskInstancing.MainDatabaseOnly, MigrationTaskRunPattern.Repeated)
        {

        }

        /// <summary>
        /// Create a copy
        /// </summary>
        public override MigrationTaskBase Clone()
        {
            return new ConvertStatusCodesMigrationTask(this);
        }

        /// <summary>
        /// Saves a converted status code blob back to the database
        /// </summary>
        private void WriteConvertedXml(SqlCommand cmd, CurrentConversion conversion)
        {
            string tableName = "GenericStore";
            if (conversion.StoreTypeCode == (int)StoreTypeCode.AmeriCommerce)
            {
                tableName = "AmeriCommerceStore";
            }
            else if (conversion.StoreTypeCode == (int)StoreTypeCode.NetworkSolutions)
            {
                tableName = "NetworkSolutionsStore";
            }

            // update the database with the new xml
            cmd.Parameters.Clear();
            cmd.CommandText = String.Format("UPDATE dbo.{0} SET StatusCodes = @statusCodes WHERE StoreID = @storeID", tableName);
            cmd.Parameters.AddWithValue("@statusCodes", conversion.ConvertedStatusBlob);
            cmd.Parameters.AddWithValue("@storeID", conversion.StoreID);

            // run the query
            SqlCommandProvider.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Execute
        /// </summary>
        protected override int Run()
        {                
            Progress.Detail = "Upgrading store status codes...";

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, SqlCommandProvider.DefaultTimeout))
            {
                using (SqlConnection con = MigrationTaskBase.OpenConnectionForTask(this))
                {
                    // get all stores, ID, type and status code
                    CurrentConversion conversion = null;
                    using (SqlCommand cmd = SqlCommandProvider.Create(con))
                    {
                        // get the next row to work on
                        cmd.CommandText = "SELECT TOP 1 StatusTempID, StoreID, TypeCode, OnlineStatusCodes FROM v2m_StoreStatusTemp";

                        long rowId = 0;
                        using (SqlDataReader reader = SqlCommandProvider.ExecuteReader(cmd))
                        {
                            if (reader.Read())
                            {
                                rowId = (long)reader["StatusTempID"];

                                conversion = new CurrentConversion()
                                {
                                    StoreID = (long)reader["StoreID"],
                                    StoreTypeCode = (int)reader["TypeCode"],
                                    StatusBlob = (string)reader["OnlineStatusCodes"]
                                };

                                // Special case for XCart - in V2 they were not saved in the database, they were saved
                                if (conversion.StoreTypeCode == (int) StoreTypeCode.XCart)
                                {
                                    conversion.StatusBlob = 
                                        @"<StatusCodes>
                                              <Status><Code>B</Code><Name>Backordered</Name></Status>
                                              <Status><Code>C</Code><Name>Complete</Name></Status>
                                              <Status><Code>D</Code><Name>Declined</Name></Status>
                                              <Status><Code>F</Code><Name>Failed</Name></Status>
                                              <Status><Code>I</Code><Name>Not Finished</Name></Status>
                                              <Status><Code>P</Code><Name>Processed</Name></Status>
                                              <Status><Code>Q</Code><Name>Queued</Name></Status>
                                        </StatusCodes>";
                                }

                                log.InfoFormat("Fetched next status code blob to convert, StoreID = {0}", conversion.StoreID);
                            }
                            else
                            {
                                // none, signal that we are done
                                log.Info("No more status code blobs to convert, exiting.");
                                return 0;
                            }
                        }

                        // Convert the XML
                        log.Info("Converting status code Xml.");
                        ConversionOnlineStatusCodeProvider statusConverter = new ConversionOnlineStatusCodeProvider(conversion.StoreTypeCode, conversion.StatusBlob);
                        conversion.ConvertedStatusBlob = statusConverter.GetShipWorks3Xml();

                        // Write it to the database
                        WriteConvertedXml(cmd, conversion);

                        // Update Orders
                        UpdateOrders(cmd, conversion, statusConverter);

                        // delete the temp status row
                        SqlCommandProvider.ExecuteNonQuery(con, "DELETE FROM v2m_StoreStatusTemp WHERE StatusTempID = " + rowId);
                        
                        // done with the transaction
                        scope.Complete();

                        // signal that we need to keep going
                        return 1;    
                    }
                }
            }
        }

        /// <summary>
        /// Update V3 order records to convert from the status code to the diaplay text.
        /// </summary>
        private void UpdateOrders(SqlCommand cmd, CurrentConversion conversion, ConversionOnlineStatusCodeProvider statusConverter)
        {
            // For XCart the "Codes" saved in the database are actually the full names.  For v3 we need to update them to be the actual codes
            if (conversion.StoreTypeCode == (int) StoreTypeCode.XCart)
            {
                foreach (string code in statusConverter.CodeValues)
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "UPDATE [Order] SET OnlineStatusCode = @code WHERE OnlineStatusCode = @name AND StoreID = @storeID";
                    cmd.Parameters.AddWithValue("@code", code);
                    cmd.Parameters.AddWithValue("@name", statusConverter.GetCodeName(code));
                    cmd.Parameters.AddWithValue("@storeID", conversion.StoreID);

                    SqlCommandProvider.ExecuteNonQuery(cmd);
                }
            }

            // update the Order table
            cmd.Parameters.Clear();
            cmd.CommandText = "UPDATE [Order] SET " +
                              "OnlineStatus = @Status " +
                              "WHERE StoreID = @StoreID " +
                              "AND CAST(OnlineStatusCode AS varchar(100)) = @StatusCode";

            cmd.Parameters.Add("@Status", SqlDbType.NVarChar);
            cmd.Parameters.Add("@StoreID", SqlDbType.BigInt);
            cmd.Parameters.Add("@StatusCode", SqlDbType.NVarChar);

            foreach (string code in statusConverter.CodeValues)
            {
                string displayText = statusConverter[code];

                log.InfoFormat("Translating order status code {0} to '{1}' for StoreID {2}...", code, displayText, conversion.StoreID);
                log.Info(cmd.CommandText);

                // update the Order table
                cmd.Parameters["@Status"].Value = displayText;
                cmd.Parameters["@StoreID"].Value = conversion.StoreID;
                cmd.Parameters["@StatusCode"].Value = code;

                SqlCommandProvider.ExecuteNonQuery(cmd);
            }

            log.Info("Done updating Order.OrderStatus");
        }

        /// <summary>
        /// Get the work estimate
        /// </summary>
        protected override int RunEstimate(SqlConnection con)
        {
            // just count each store as a work unit
            using (SqlCommand cmd = SqlCommandProvider.Create(con))
            {
                try
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM Stores";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
                catch (SqlException)
                {
                    cmd.CommandText = "SELECT COUNT(*) FROM v2m_StoreStatusTemp";

                    return (int)SqlCommandProvider.ExecuteScalar(cmd);
                }
            }
        }
    }
}
