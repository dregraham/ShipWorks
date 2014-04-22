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
                -- Only execute the trigger if the ShipSense hash key of an order has changed to avoid deadlocks
                -- that could occur while processing the shipment and the order's local status gets updated
                IF (UPDATE(ShipSenseHashKey))
                BEGIN
	                -- If we do not match a ShipSenseKnowledgeBase Hash, then always update the shipment shipsense status to 0, Not Applied
	                -- for all shipments assigned to the inserted order
	                IF NOT EXISTS (SELECT * FROM ShipSenseKnowledgeBase sskb, INSERTED i WHERE sskb.[Hash] = i.ShipSenseHashKey)
	                BEGIN
		                UPDATE s
		                SET s.ShipSenseStatus = 0
		                FROM Shipment s, INSERTED i
		                WHERE s.OrderID = i.OrderID
		                    AND s.Processed = 0
		                    AND s.ShipSenseStatus <> 0
	                END
	                ELSE
	                BEGIN
		                -- We have a ShipSenseKnowledgeBase entry based on Hash, so now we check shipments for the inserted order
		                UPDATE s
		                SET s.ShipSenseStatus =
			                CASE
				                WHEN sskb.Entry = s.ShipSenseEntry THEN 1
				                ELSE 2
			                END
		                FROM Shipment s
			                INNER JOIN
			                    INSERTED i 
                                ON i.OrderID = s.OrderID
			                INNER JOIN 
                                ShipSenseKnowledgeBase sskb WITH(NOLOCK) 
                                ON sskb.Hash = i.ShipSenseHashKey
		                WHERE 
                            s.Processed = 0
		                    AND s.ShipSenseStatus <> 0
	                END
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
