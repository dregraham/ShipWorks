using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;

public partial class Triggers
{
    [SqlTrigger(Target = "Order", Event = "FOR INSERT, UPDATE")]
    public static void OrderShipSensibleTrigger ()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            string sql = @"
                -- Update the ShipSenseRecognition status based on a matching ShipSenseKnowledgeBase record

                -- NOTE: Since we are in an Order table trigger, and we are updating orders, we need to do the where
                -- where o.ShipSensible = X below so that we don't get in a infinite recursive loop

	            if EXISTS(select * from ShipSenseKnowledgeBase sskb, inserted i where sskb.[Hash] = i.ShipSenseHashKey)
	            BEGIN
                    -- The hash key is in the KB; update the status to recognized if it wasn't already
		            update o 
		            set ShipSenseRecognitionStatus = 1 
		            from [Order] o
		            join inserted i on i.OrderID = o.OrderID
		            where o.ShipSenseRecognitionStatus != 1
	            END
	            ELSE
	            BEGIN
                    -- The hash key is not in the KB; update the status to not recognized
		            update o 
		            set ShipSenseRecognitionStatus = 0
		            from [Order] o
		            join inserted i on i.OrderID = o.OrderID
		            where o.ShipSenseRecognitionStatus = 1
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
