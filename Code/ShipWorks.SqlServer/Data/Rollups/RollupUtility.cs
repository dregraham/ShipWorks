using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using System.Diagnostics;

namespace ShipWorks.SqlServer.Data.Rollups
{
    /// <summary>
    /// Utility class for calculating rollups
    /// </summary>
    public static class RollupUtility
    {
        /// <summary>
        /// Calculate the rollup columns in response to a trigger on "childTable".  The "parentTable" is where the rollup columns are stored.  "childCountColumn"
        /// is a column in parent table that tracks how many total child objects there are.
        /// </summary>
        public static void UpdateRollups(string parentTable, string parentKey, string childCountColumn, string childTable, List<RollupColumn> rollupColumns)
        {
            TriggerAction triggerAction = SqlContext.TriggerContext.TriggerAction;

            // Attach to the connection
            using (SqlConnection con = new SqlConnection("Context connection = true"))
            {
                con.Open();

                Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, UtilityFunctions.GetTriggerTableName());

                // If its an update, and the parent changed, we have to pretend like we were deleted from the previous parent and added to the new parent
                if (triggerAction == TriggerAction.Update && SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap[parentKey]))
                {
                    PerformRollups(con, TriggerAction.Delete, "deleted", parentTable, parentKey, childCountColumn, childTable, rollupColumns);
                    PerformRollups(con, TriggerAction.Insert, "inserted", parentTable, parentKey, childCountColumn, childTable, rollupColumns);
                }
                else
                {
                    PerformRollups(con, triggerAction, UtilityFunctions.GetTriggerTableName(), parentTable, parentKey, childCountColumn, childTable, rollupColumns);
                }
            }
        }

        /// <summary>
        /// Perform the rollup
        /// </summary>
        private static void PerformRollups(SqlConnection con, TriggerAction triggerAction, string triggerTable, string parentTable, string parentKey, string childCountColumn, string childTable, List<RollupColumn> rollupColumns)
        {
            // We need to new which column names go with which ordinals
            Dictionary<string, int> ordinalMap = UtilityFunctions.GetColumnNameOrdinalMap(con, triggerTable);

            // If its an update, and no affected columns changed, then we don't do anywhing
            if (triggerAction == TriggerAction.Update)
            {
                bool anyColumnsAffected = false;

                foreach (RollupColumn column in rollupColumns)
                {
                    if (IsSourceColumnAffected(column, ordinalMap))
                    {
                        anyColumnsAffected = true;
                    }
                }

                if (!anyColumnsAffected)
                {
                    return;
                }
            }

            bool previousSumsRequired = false;

            // Select all the rollup columns
            StringBuilder sqlColumnList = new StringBuilder();
            foreach (RollupColumn column in rollupColumns)
            {
                // If its an update, and this column didn't actually change, we don't do anything
                if (triggerAction == TriggerAction.Update && !IsSourceColumnAffected(column, ordinalMap))
                {
                    continue;
                }

                if (sqlColumnList.Length > 0)
                {
                    sqlColumnList.Append(", ");
                }

                // Single\Same or Null
                if (column.Method == RollupMethod.SingleOrNull ||
                    column.Method == RollupMethod.SameOrNull)
                {
                    string countClause = (column.Method == RollupMethod.SingleOrNull) ? "COUNT(*)" : string.Format("COUNT(DISTINCT(t.{0}))", column.SourceColumn);

                    if (column.IsBitField)
                    {
                        sqlColumnList.AppendFormat("CASE {1} WHEN 1 THEN SUM(CAST(t.{0} as INT)) ELSE NULL END as {0}", column.SourceColumn, countClause);
                    }
                    else
                    {
                        sqlColumnList.AppendFormat("CASE {1} WHEN 1 THEN MAX(t.{0}) ELSE NULL END as {0}", column.SourceColumn, countClause);
                    }
                }
                else
                {
                    sqlColumnList.AppendFormat("SUM(COALESCE(t.{0}, 0)) as {0}", column.SourceColumn);

                    // If its an update we also need to know what the sum used to be, so that we can know what the difference is
                    if (triggerAction == TriggerAction.Update)
                    {
                        sqlColumnList.AppendFormat(", SUM(COALESCE(d.{0}, 0)) as {0}Previous", column.SourceColumn);

                        previousSumsRequired = true;
                    }
                }
            }

            string joinOnDeleted = previousSumsRequired ? " INNER JOIN deleted d ON d.$IDENTITY = t.$IDENTITY " : " ";

            // Create the table to hold the results of this current operation
            SqlCommand setupTablesCmd = con.CreateCommand();
            setupTablesCmd.CommandText = string.Format(@"

                SELECT t.{0}, COUNT(*) as ChangeCount, {1}
                   INTO #Rollups
                   FROM {2} t {3}
                   GROUP BY t.{0};

                CREATE UNIQUE NONCLUSTERED INDEX IX_Rollups ON #Rollups ({0});

                ", parentKey, sqlColumnList, triggerTable, joinOnDeleted);
            setupTablesCmd.ExecuteNonQuery();

            StringBuilder setColumns = new StringBuilder();

            // For insertes\deletes, we have to update the running child count column
            if (triggerAction != TriggerAction.Update)
            {
                setColumns.AppendFormat("[{0}].{1} = [{0}].{1} {2} r.ChangeCount", parentTable, childCountColumn, triggerAction == TriggerAction.Delete ? "-" : "+");
            }

            // Add in all the columns that need updated
            foreach (RollupColumn column in rollupColumns)
            {
                // If its an update, and this column didn't actually change, we don't do anything
                if (triggerAction == TriggerAction.Update && !IsSourceColumnAffected(column, ordinalMap))
                {
                    continue;
                }

                if (setColumns.Length > 0)
                {
                    setColumns.AppendLine(",");
                }

                // Trigger was for an update
                if (triggerAction == TriggerAction.Update)
                {
                    if (column.Method == RollupMethod.SingleOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE [{0}].{2} WHEN 1 THEN r.{3} ELSE NULL END", parentTable, column.TargetColumn, childCountColumn, column.SourceColumn);
                    }
                    else if (column.Method == RollupMethod.SameOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE WHEN [{0}].{2} = 1 THEN r.{3} WHEN (SELECT COUNT(DISTINCT(c.{3})) FROM [{4}] c WHERE c.{5} = r.{5}) = 1 THEN r.{3} ELSE NULL END", parentTable, column.TargetColumn, childCountColumn, column.SourceColumn, childTable, parentKey);
                    }
                    else
                    {
                        setColumns.AppendFormat("[{0}].{1} = COALESCE([{0}].{1}, 0) + COALESCE((r.{2} - r.{2}Previous), 0)", parentTable, column.TargetColumn, column.SourceColumn);
                    }
                }

                // Trigger was for an insert
                if (triggerAction == TriggerAction.Insert)
                {
                    if (column.Method == RollupMethod.SingleOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE [{0}].{2} + r.ChangeCount WHEN 1 THEN r.{3} ELSE NULL END", parentTable, column.TargetColumn, childCountColumn, column.SourceColumn);
                    }
                    else if (column.Method == RollupMethod.SameOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE WHEN [{0}].{2} + r.ChangeCount = 1 THEN r.{3} WHEN [{0}].{1} = r.{3} THEN r.{3} ELSE NULL END", parentTable, column.TargetColumn, childCountColumn, column.SourceColumn);
                    }
                    else
                    {
                        setColumns.AppendFormat("[{0}].{1} = COALESCE([{0}].{1}, 0) + COALESCE(r.{2}, 0)", parentTable, column.TargetColumn, column.SourceColumn);
                    }
                }

                // Trigger was for a delete
                if (triggerAction == TriggerAction.Delete)
                {
                    if (column.Method == RollupMethod.SingleOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE [{0}].{2} - r.ChangeCount WHEN 1 THEN (SELECT TOP(1) x.{3} FROM {4} x WHERE [{0}].{5} = x.{5}) ELSE NULL END",
                            parentTable, column.TargetColumn, childCountColumn, column.SourceColumn, childTable, parentKey);
                    }
                    else if (column.Method == RollupMethod.SameOrNull)
                    {
                        setColumns.AppendFormat("[{0}].{1} = CASE WHEN [{0}].{2} - r.ChangeCount = 0 THEN NULL WHEN (SELECT COUNT(DISTINCT(c.{3})) FROM [{4}] c WHERE c.{5} = r.{5}) = 1 THEN (SELECT TOP(1) c.{3} FROM [{4}] c WHERE c.{5} = r.{5}) ELSE NULL END",
                            parentTable, column.TargetColumn, childCountColumn, column.SourceColumn, childTable, parentKey);
                    }
                    else
                    {
                        setColumns.AppendFormat("[{0}].{1} = COALESCE([{0}].{1}, 0) - COALESCE(r.{2}, 0)", parentTable, column.TargetColumn, column.SourceColumn);
                    }
                }
            }

            // Shouldnt be, otherwise we would have bailed earlier in the function
            if (setColumns.Length == 0)
            {
                return;
            }


            SqlCommand updateCmd = con.CreateCommand();
            updateCmd.CommandText = string.Format(@"
                UPDATE [{0}]
                  SET {1}
                  FROM #Rollups r
                  WHERE r.{2} = [{0}].{2}",
                parentTable, setColumns, parentKey);
            updateCmd.ExecuteNonQuery();

            SqlCommand cleanupCmd = con.CreateCommand();
            cleanupCmd.CommandText = "DROP TABLE #Rollups";
            cleanupCmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Determines if the SourceColumn of the given RollupColunn is affected by the current triggering action
        /// </summary>
        private static bool IsSourceColumnAffected(RollupColumn column, Dictionary<string, int> ordinalMap)
        {
            if (column.SourceDependencies == null)
            {
                return SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap[column.SourceColumn]);
            }
            else
            {
                return column.SourceDependencies.Any(name => SqlContext.TriggerContext.IsUpdatedColumn(ordinalMap[name]));
            }
        }
    }
}
