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
                -- Update the ShipSensible bit based on a matching ShipSenseKnowledgeBase record

-- NOTE: Since we are in an Order table trigger, and we are updating orders, we need to do the where
-- where o.ShipSensible = X below so that we don't get in a infinite recursive loop

	            if EXISTS(select * from ShipSenseKnowledgeBase sskb, inserted i where sskb.[Hash] = i.ShipSenseHashKey)
	            BEGIN
		            update o 
		            set ShipSensible = 1 
		            from [Order] o
		            join inserted i on i.OrderID = o.OrderID
		            where o.ShipSensible = 0
	            END
	            ELSE
	            BEGIN
		            update o 
		            set ShipSensible = 0
		            from [Order] o
		            join inserted i on i.OrderID = o.OrderID
		            where o.ShipSensible = 1
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
