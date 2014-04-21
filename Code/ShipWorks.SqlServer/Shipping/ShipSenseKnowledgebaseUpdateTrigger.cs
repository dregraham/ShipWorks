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
		                        [Order] o with(nolock) on o.OrderID = s.OrderID
		                        join
		                        inserted i on i.Hash = o.ShipSenseHashKey
	                        where s.Processed = 0
	                          and s.ShipSenseStatus <> 0

                            update o
                            set o.ShipSensible = 1 
                            from [Order] o
                                join inserted i on i.Hash = o.ShipSenseHashKey
                            and o.ShipSensible = 0
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
