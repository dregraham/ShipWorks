using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;
using ShipWorks.SqlServer.Common.Data;

public partial class Triggers
{
    [SqlTrigger(Target = "ShipSenseKnowledgebase", Event = "FOR INSERT,UPDATE")]
    public static void ShipSenseKnowledgebaseUpdateTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            string sql = @"
	                        update s
	                        set s.ShipSenseStatus = 
		                        case
			                        when s.ShipSenseEntry = i.Entry then 1
			                        else 2
		                        end
	                        from Shipment s
		                        join
		                        inserted i on i.Hash = s.ShipSenseHashKey
	                        where s.Processed = 0
	                          and s.ShipSenseStatus <> 0
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
