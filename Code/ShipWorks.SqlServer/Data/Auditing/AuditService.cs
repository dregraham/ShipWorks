using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ShipWorks.SqlServer.General;
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Data;
using ShipWorks.Users.Audit;

namespace ShipWorks.SqlServer.Data.Auditing
{
    /// <summary>
    /// Provides audit functionality within SQL Server
    /// 
    /// Using the CompilerGenerated hack to enable use of static (not readonly) variables without
    /// specifying an UNSAFE assembly
    /// </summary>
    [CompilerGenerated]
    public static class AuditService
    {        
        // A map of table names to their audit information
        static Dictionary<string, AuditTableInfo> tableMap = null;

        /// <summary>
        /// Audit the data from the currently executing trigger
        /// </summary>
        public static void AuditExecutingTrigger(string tableName)
        {
            // Attach to the connection
            using (SqlConnection con = new SqlConnection("Context connection = true"))
            {
                con.Open();

                if (UtilityFunctions.GetLastRowCount(con) == 0)
                {
                    return;
                }

                AuditTableInfo tableInfo = GetAuditTableInfo(con, tableName);

                // Determine which columns we need to audit
                List<string> auditColumns = DetermineAuditColumns(tableInfo);

                // If there are no audit table columns, then get out
                if (auditColumns.Count == 0)
                {
                    return;
                }

                // Get the currently executing context
                UserContext userContext = UtilityFunctions.GetUserContext(con);

                // Quit if auditing is not enable
                if (DetermineAuditState(tableName, userContext, con) == AuditState.Disabled)
                {
                    return;
                }

                long auditID = CreateOrGetAuditID(con, userContext);

                // Create the audit-changes - one for each row that is changing
                CreateAuditChanges(con, userContext, tableInfo, auditID, auditColumns);
            }
        }

        /// <summary>
        /// Determines the audit state.  This is now needed as we try to reuse connections, and therefore 
        /// connection strings which hold audit state.  But when inserting Orders/Customers, we may have a
        /// connection string that doesn't have the correct audit scope detail, like don't audit new orders
        /// or customers.  
        /// 
        /// This method will determine if the TriggerAction is an insert and if the table is an Order related
        /// table, Customer or Note, and check the Configuration.AuditNewOrders to determine if we should
        /// switch to Disabled.
        /// 
        /// </summary>
        private static AuditState DetermineAuditState(string tableName, UserContext userContext, SqlConnection con)
        {
            // Quit if auditing is not enable
            if (userContext.AuditState == AuditState.Disabled)
            {
                return AuditState.Disabled;
            }

            // Now check for inserted Order tables or Customer table
            bool isInsert = SqlContext.TriggerContext?.TriggerAction == TriggerAction.Insert;
            if (isInsert && 
                (tableName.IndexOf("Order", StringComparison.InvariantCultureIgnoreCase) >= 0 ||
                 tableName.Equals("Customer", StringComparison.InvariantCultureIgnoreCase) ||
                 tableName.Equals("Note", StringComparison.InvariantCultureIgnoreCase)))
            {
                object auditNewOrders;
                using (SqlCommand findCmd = con.CreateCommand())
                {
                    findCmd.CommandText = "SELECT AuditNewOrders FROM Configuration";
                    auditNewOrders = findCmd.ExecuteScalar();
                }

                AuditState auditState = (bool) auditNewOrders ? AuditState.Default : AuditState.Disabled;
                return auditState;
            }

            return userContext.AuditState;
        }

        /// <summary>
        /// Create a new Audit entry or pull from the existing one for the transaction
        /// </summary>
        private static long CreateOrGetAuditID(SqlConnection con, UserContext userContext)
        {
            long transactionID = UtilityFunctions.GetTransactionID(con);

            SqlCommand findCmd = con.CreateCommand();
            findCmd.CommandText = "SELECT AuditID FROM Audit WHERE TransactionID = @transactionID";
            findCmd.Parameters.AddWithValue("@transactionID", transactionID);
            object result = findCmd.ExecuteScalar();

            if (result != null && !(result is DBNull))
            {
                return (long) result;
            }

            SqlCommand insertCmd = con.CreateCommand();
            insertCmd.CommandText = @"
                INSERT INTO Audit
                   (
                        TransactionID,
                        UserID,
                        ComputerID,
                        Reason,
                        ReasonDetail,
                        Date,
                        Action,
                        ObjectID,
                        HasEvents
                    )
                OUTPUT
                    inserted.AuditID
                VALUES
                    (
                        @TransactionID,
                        @UserID,
                        @ComputerID,
                        @Reason,
                        @ReasonDetail,
                        GETUTCDATE(),
                        0,
                        NULL,
                        1
                  )";

            insertCmd.Parameters.AddWithValue("@TransactionID", transactionID);
            insertCmd.Parameters.AddWithValue("@UserID", userContext.UserID);
            insertCmd.Parameters.AddWithValue("@ComputerID", userContext.ComputerID);
            insertCmd.Parameters.AddWithValue("@Reason", userContext.AuditReasonType);
            insertCmd.Parameters.AddWithValue("@ReasonDetail", userContext.AuditReasonDetail);

            return (long) insertCmd.ExecuteScalar();
        }

        /// <summary>
        /// Create the parent audit-change
        /// </summary>
        private static void CreateAuditChanges(SqlConnection con, UserContext userContext, AuditTableInfo tableInfo, long auditID, List<string> auditColumns)
        {
            // We need the PK's of the object that's changing
            foreach (long key in UtilityFunctions.GetTriggerPrimaryKeys(con, tableInfo.PrimaryKey))
            {
                SqlCommand insertCmd = con.CreateCommand();
                insertCmd.CommandText = @"
                     INSERT INTO AuditChange
                         (
                            AuditID,
                            ChangeType,
                            ObjectID
                         )
                     OUTPUT
                        inserted.AuditChangeID
                     VALUES
                        (
                            @AuditID,
                            @ChangeType,
                            @ObjectID
                        )";

                insertCmd.Parameters.AddWithValue("@AuditID", auditID);
                insertCmd.Parameters.AddWithValue("@ChangeType", (int) SqlContext.TriggerContext.TriggerAction);
                insertCmd.Parameters.AddWithValue("@ObjectID", key);

                long auditChangeID = (long) insertCmd.ExecuteScalar();

                // Only log full details if auditing is fully enabled
                if (userContext.AuditState == AuditState.Enabled)
                {
                    // Log the details of the event - all the column before\after's
                    bool anyDetails = CreateAuditChangeDetails(con, key, auditID, auditChangeID, auditColumns, tableInfo);

                    // If there are not any details, delete this change
                    if (!anyDetails)
                    {
                        SqlCommand deleteCmd = con.CreateCommand();
                        deleteCmd.CommandText = "DELETE AuditChange WHERE AuditChangeID = @auditChangeID";
                        deleteCmd.Parameters.AddWithValue("@auditChangeID", auditChangeID);
                        deleteCmd.ExecuteNonQuery();
                    }
                }
            }
        }

        /// <summary>
        /// Determine which columns need to be audited
        /// </summary>
        private static List<string> DetermineAuditColumns(AuditTableInfo tableInfo)
        {
            const byte VarBinaryDataType = 165;

            TriggerAction triggerAction = SqlContext.TriggerContext.TriggerAction;

            // List of columns we are going to audit for this event
            List<string> auditColumns = new List<string>();

            // Go through each column in the table
            foreach (AuditColumnInfo columnInfo in tableInfo.ColumnInfo.Select(p => p.Value))
            {
                // Skip rowversions
                if (columnInfo.ColumnName == "RowVersion")
                {
                    continue;
                }

                // Skip rollups
                if (columnInfo.ColumnName.StartsWith("Rollup"))
                {
                    continue;
                }

                // Skip varbinary columns
                if (columnInfo.DataType == VarBinaryDataType)
                {
                    continue;
                }

                // If it was updated (or if being deleted or inserted), we log it
                if (triggerAction != TriggerAction.Update || SqlContext.TriggerContext.IsUpdatedColumn(columnInfo.Ordinal))
                {
                    auditColumns.Add(columnInfo.ColumnName);
                }
            }

            return auditColumns;
        }

        /// <summary>
        /// Create the event detail - each column that has changed.
        /// </summary>
        private static bool CreateAuditChangeDetails(SqlConnection con, long key, long auditID, long auditChangeID, List<string> auditColumns, AuditTableInfo tableInfo)
        {
            StringBuilder selectList = new StringBuilder();
            foreach (string column in auditColumns)
            {
                if (selectList.Length > 0)
                {
                    selectList.Append(", ");
                }

                selectList.AppendFormat("D.{0} as Old{0}, I.{0} as New{0}", column);
            }

            SqlCommand dataCmd = con.CreateCommand();
            dataCmd.CommandText = string.Format(@"
                    SELECT {0} 
                    FROM inserted I FULL OUTER JOIN deleted D on I.{2} = D.{2}
                    WHERE {1}.{2} = @key", 
                selectList, 
                SqlContext.TriggerContext.TriggerAction == TriggerAction.Delete ? "D" : "I",
                tableInfo.PrimaryKey);

            dataCmd.Parameters.AddWithValue("@key", key);

            object[] values;

            // Go through each row that changed
            using (SqlDataReader reader = dataCmd.ExecuteReader())
            {
                values = new object[reader.FieldCount];

                reader.Read();
                reader.GetValues(values);
            }

            int index = 0;
            int auditCount = 0;

            // Go through each column to audit
            foreach (string column in auditColumns)
            {
                AuditColumnInfo columnInfo = tableInfo.ColumnInfo[column];

                object oldValue = values[index];
                object newValue = values[index + 1];

                // Update for next loop
                index += 2;

                if (oldValue is DBNull) oldValue = null;
                if (newValue is DBNull) newValue = null;

                string oldText = null;
                string newText = null;

                object oldVariant = null;
                object newVariant = null;

                // Both null, nothing changed, skip auditing
                if (oldValue == null && newValue == null)
                {
                    continue;
                }

                // Check if they are not null, and are the same
                if (oldValue != null && newValue != null)
                {
                    if (oldValue.Equals(newValue))
                    {
                        continue;
                    }
                }

                // Store text over 100 chars in the text column, numerics and short text can go in the variant
                if (columnInfo.IsText && 
                   (
                        (oldValue != null && oldValue.ToString().Length > 100) ||
                        (newValue != null && newValue.ToString().Length > 100)
                   ))
                {
                    oldText = (string) oldValue;
                    newText = (string) newValue;
                }
                else
                {
                    oldVariant = oldValue;
                    newVariant = newValue;
                }

                auditCount++;

                SqlCommand insertCmd = con.CreateCommand();
                insertCmd.CommandText = @"
                    INSERT INTO AuditChangeDetail
                        (
                            AuditChangeID,
                            AuditID,
                            DisplayName,
                            DisplayFormat,
                            DataType,
                            TextOld,
                            TextNew,
                            VariantOld,
                            VariantNew
                        )
                    VALUES
                        (
                            @AuditChangeID,
                            @AuditID,
                            @DisplayName,
                            @DisplayFormat,
                            @DataType,
                            @TextOld,
                            @TextNew,
                            @VariantOld,
                            @VariantNew
                        )";

                insertCmd.Parameters.AddWithValue("@AuditChangeID", auditChangeID);
                insertCmd.Parameters.AddWithValue("@AuditID", auditID);
                insertCmd.Parameters.AddWithValue("@DisplayName", columnInfo.DisplayName);
                insertCmd.Parameters.AddWithValue("@DisplayFormat", columnInfo.DisplayFormat);
                insertCmd.Parameters.AddWithValue("@DataType", columnInfo.DataType);
                insertCmd.Parameters.AddWithValue("@TextOld", oldText);
                insertCmd.Parameters.AddWithValue("@TextNew", newText);
                insertCmd.Parameters.AddWithValue("@VariantOld", oldVariant);
                insertCmd.Parameters.AddWithValue("@VariantNew", newVariant);

                insertCmd.ExecuteNonQuery();
            }

            return auditCount > 0;
        }


        /// <summary>
        /// Get the SQL server object ID for the given table
        /// </summary>
        private static AuditTableInfo GetAuditTableInfo(SqlConnection con, string tableName)
        {
            // CLR ensures this is thread safe
            if (tableMap == null)
            {
                // Create new local map and load it up
                Dictionary<string, AuditTableInfo> localMap = new Dictionary<string, AuditTableInfo>();

                SqlDataAdapter adapter = new SqlDataAdapter("SELECT object_id, name FROM sys.tables", con);
                DataTable tableIDs = new DataTable();
                adapter.Fill(tableIDs);

                foreach (var row in tableIDs.Rows.Cast<DataRow>())
                {
                    AuditTableInfo tableInfo = new AuditTableInfo { TableID = Convert.ToInt64(row[0]), TableName = (string) row[1] };

                    // Load the column information
                    LoadAuditColumnInfo(con, tableInfo);

                    // With the column info we can determine the PK
                    tableInfo.PrimaryKey = tableInfo.ColumnInfo.Where(p => p.Value.Ordinal == 0).Single().Value.ColumnName;

                    // Cache it for next time
                    localMap[tableInfo.TableName] = tableInfo;
                }

                // Now automically set the global map
                Interlocked.CompareExchange<Dictionary<string, AuditTableInfo>>(ref tableMap, localMap, null);
            }

            return tableMap[tableName];
        }

        /// <summary>
        /// Load the sql system column information for the given set of columns
        /// </summary>
        private static void LoadAuditColumnInfo(SqlConnection con, AuditTableInfo tableInfo)
        {
            Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, tableInfo.TableName);

            SqlCommand columnCmd = con.CreateCommand();
            columnCmd.CommandText = @"
                SELECT c.column_id, 
                        MAX(c.name), 
                        MAX(c.system_type_id),
                        MAX(CASE p.name WHEN 'AuditFormat' THEN p.value END) as AuditFormat, 
                        MAX(CASE p.name WHEN 'AuditName' THEN p.value END) as AuditName, 
                        MAX(f.referenced_column_id)
                   FROM sys.columns c 
                        LEFT OUTER JOIN sys.extended_properties p ON c.object_id = p.major_id AND c.column_id = p.minor_id
                        LEFT OUTER JOIN sys.foreign_key_columns f ON f.parent_column_id = c.column_id AND f.parent_object_id = c.object_id
                   WHERE object_id = @tableID
                   GROUP BY c.column_id";

            columnCmd.Parameters.AddWithValue("@tableID", tableInfo.TableID);

            using (SqlDataReader reader = columnCmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    int columnID = (int) reader[0];
                    string name = (string) reader[1];
                    byte dataType = (byte) reader[2];

                    object auditFormat = reader[3];
                    object auditName = reader[4];

                    bool isForeignKey = !(reader[5] == null || reader[5] is DBNull);

                    if (auditFormat == null || auditFormat is DBNull)
                    {
                        // PK and FK fields don't get displayed by default (if there was an Extended Property, it overrides)
                        if (name == tableInfo.TableName + "ID" || isForeignKey)
                        {
                            auditFormat = 1;
                        }
                        else
                        {
                            // Default display
                            auditFormat = 0;
                        }
                    }

                    if (auditName == null || auditName is DBNull)
                    {
                        auditName = name;
                    }

                    tableInfo.ColumnInfo[name] = new AuditColumnInfo
                        {
                            ColumnID = columnID,
                            ColumnName = name,
                            Ordinal = ordinalMap[name],
                            DataType = dataType,
                            DisplayFormat = Convert.ToByte(auditFormat),
                            DisplayName = (string) auditName
                        };
                }
            }
        }
    }
}
