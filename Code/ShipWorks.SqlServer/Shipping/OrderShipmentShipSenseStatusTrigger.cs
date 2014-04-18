using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    [SqlTrigger(Target = "Order", Event = "FOR UPDATE")]
    public static void OrderShipmentShipSenseStatusTrigger()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            string sql = @"
	            -- If we do not match a ShipSenseKnowledgeBase Hash, then always update the shipment shipsense status to 0, Not Applied
	            -- for all shipments assigned to the inserted order
	            if not exists (select * from ShipSenseKnowledgeBase sskb, inserted i where sskb.[Hash] = i.ShipSenseHashKey)
	            begin
		            update s
		            set s.ShipSenseStatus = 0
		            from Shipment s, inserted i
		            where s.OrderID = i.OrderID
		                and s.Processed = 0
		                and s.ShipSenseStatus <> 0
	            end
	            else
	            begin
		            -- We have a ShipSenseKnowledgeBase entry based on Hash, so now we check shipments for the inserted order
		            update s
		            set s.ShipSenseStatus =
			            case
				            when sskb.Entry = s.ShipSenseEntry then 1
				            else 2
			            end
		            from Shipment s
			            join
			            inserted i on i.OrderID = s.OrderID
			            join
			            ShipSenseKnowledgeBase sskb with(nolock) on sskb.Hash = i.ShipSenseHashKey
		            where s.Processed = 0
		                and s.ShipSenseStatus <> 0
	            end
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
