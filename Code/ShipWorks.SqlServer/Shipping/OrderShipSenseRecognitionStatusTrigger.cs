using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    [SqlTrigger(Target = "Order", Event = "FOR INSERT, UPDATE")]
    public static void OrderShipSenseRecognitionStatusTrigger ()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            string sql = @"
                -- Update the ShipSenseRecognition status based on a matching ShipSenseKnowledgeBase record

                -- NOTE: Since we are in an Order table trigger, and we are updating orders, we need to do the where
                -- where o.ShipSenseRecognitionStatus = X below so that we don't get in a infinite recursive loop

                -- Only execute the trigger if the ShipSense hash key of an order has changed to avoid deadlocks
                -- that could occur while processing the shipment and the order's local status gets updated
                IF (UPDATE(ShipSenseHashKey))
                BEGIN
	                If EXISTS(SELECT 1 FROM ShipSenseKnowledgeBase sskb INNER JOIN inserted i ON sskb.[Hash] = i.ShipSenseHashKey)
	                BEGIN
                        -- The hash key is in the KB; update the status to recognized if it wasn't already
		                UPDATE o 
		                SET ShipSenseRecognitionStatus = 1 
		                FROM [Order] o
		                INNER JOIN inserted i ON i.OrderID = o.OrderID
		                WHERE o.ShipSenseRecognitionStatus != 1
	                END
	                ELSE
	                BEGIN
                        -- The hash key is not in the KB; update the status to not recognized
		                UPDATE o 
		                SET ShipSenseRecognitionStatus = 0
		                FROM [Order] o
		                INNER JOIN inserted i ON i.OrderID = o.OrderID
		                WHERE o.ShipSenseRecognitionStatus = 1
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
