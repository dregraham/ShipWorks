﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using SD.LLBLGen.Pro.ORMSupportClasses;
using System.Data.SqlTypes;
using System.Diagnostics;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Class to query SQL Server for it's configuration properties.
    /// </summary>
    public static class SqlServerInfo
    {
        private const string FetchQuery = @"
        sp_configure 'show advanced options', 1
        RECONFIGURE WITH OVERRIDE;

        CREATE TABLE #MemoryValues
        (
            [Name]  sysname,
            [minimum] bigint,
            [maximum] bigint,
            [config_value] bigint,
            [run_value] bigint,
        )
        INSERT #MemoryValues EXEC sp_configure 'max server memory'
        ;with SqlInfo as
        (
            select  serverproperty('Edition') as 'SqlServer.Edition',
                    serverproperty('ProductVersion') as 'SqlServer.Product.Version',
                    serverproperty('IsFullTextInstalled') as 'SqlServer.FullText.Installed',
                    serverproperty('ProductLevel') as 'SqlServer.Product.Level',
                    serverproperty('ProductUpdateLevel') as 'SqlServer.Product.UpdateLevel',
                    serverproperty('ProductUpdateReference') as 'SqlServer.Product.UpdateReference'
        ),
        CPUs as
        (
            select  cpu_count AS [SqlServer.Cpu.Count],
	                {0} as [SqlServer.PhysicalMemory.Total]
            from sys.dm_os_sys_info
        ),
        MemoryInfo as
        (
            select  minimum   as 'SqlServer.PhysicalMemory.Allocated.Min', 
                    maximum   as 'SqlServer.PhysicalMemory.Allocated.Max', 
                    run_value as 'SqlServer.PhysicalMemory.Allocated.Running'
            from #MemoryValues
        ),
		TriggerInfo as
		(
			{1}
		)

        select SqlInfo.*, CPUs.*, MemoryInfo.*, TriggerInfo.*
        from SqlInfo, CPUs, MemoryInfo, TriggerInfo 

        drop TABLE #MemoryValues
        ";

        private const string TriggerCountQuery = @"
			select count(*) as CustomTriggerCount 
			from sys.triggers 
			where type != 'TA' and is_ms_shipped = 0 and is_disabled = 0
			  AND name != 'FilterNodeSetSwFilterNodeID'";

        /// <summary>
        /// Fetches a list of configuration properties from SQL Server
        /// </summary>
        public static IDictionary<string, string> Fetch()
        {
            Dictionary<string, string> values = new Dictionary<string, string>();

            try
            {
                string connectionString = SqlSession.Current.Configuration.GetConnectionString();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL Server 2008 has a different column name for total physical memory, so check for that
                    string memoryColumn = connection.ServerVersion.Contains("10.50") ?
                        "physical_memory_in_bytes / 1024.0 / 1024 " :
                        "physical_memory_kb / 1024.0 ";

                    using (SqlCommand cmd = connection.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = string.Format(string.Format(FetchQuery, memoryColumn, TriggerCountQuery), memoryColumn);

                        using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            if (reader.Read() && reader.HasRows)
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string value = reader[i].ToString();
                                    value = string.IsNullOrWhiteSpace(value) ? "N/A" : value;
                                    values.Add(reader.GetName(i), value);
                                }
                            }
                        }
                    }

                    // Add db size info values
                    foreach (KeyValuePair<string, string> dictionaryEntry in DbSizeInfo.Fetch(connection))
                    {
                        values.Add(dictionaryEntry.Key, dictionaryEntry.Value);
                    }
                }
            }
            catch
            {
                // We want the user to still be able to use ShipWorks even if an exception occurs, so just let this through.
            }

            return values;
        }

        /// <summary>
        /// Check the selected db to see if it has custom triggers.
        /// </summary>
        public static bool HasCustomTriggers()
        {
            using (IDbConnection connection = SqlSession.Current.OpenConnection())
            {
                using (IDbCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = TriggerCountQuery;
                    int result = Int32.Parse(cmd.ExecuteScalar().ToString());
                    return result > 0;
                }
            }
        }
    }
}
