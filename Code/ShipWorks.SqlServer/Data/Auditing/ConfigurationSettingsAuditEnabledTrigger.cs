using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target = "Configuration", Event = "FOR INSERT,UPDATE")]
    public static void ConfigurationSettingsAuditEnabledTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Disable audit triggers when AuditingEnabled is disabled, enable them when they are enabled.
            string sql = @"
                DECLARE @triggerEnableSql nvarchar(MAX) = '';
                DECLARE @triggerDisableSql nvarchar(MAX) = '';
                SELECT 
	                @triggerEnableSql += 'ALTER TABLE ' + QUOTENAME(OBJECT_NAME(parent_id)) + ' ENABLE TRIGGER ' + QUOTENAME(name) + ';' + char(13) + char(10),
	                @triggerDisableSql += 'ALTER TABLE ' + QUOTENAME(OBJECT_NAME(parent_id)) + ' DISABLE TRIGGER ' + QUOTENAME(name) + ';' + char(13) + char(10)
                FROM sys.triggers
                WHERE [Name] LIKE '%AuditTrigger'

                IF (EXISTS(SELECT AuditEnabled FROM [Configuration] WHERE AuditEnabled = 1))
	                BEGIN
		                EXEC SP_EXECUTESQL @triggerEnableSql
	                END
                ELSE
	                BEGIN
		                EXEC SP_EXECUTESQL @triggerDisableSql
	                END
                    ";

            using (SqlCommand sqlCommand = con.CreateCommand())
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.CommandText = sql;
                sqlCommand.ExecuteNonQuery();
            }
        }
    }
}
