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

            // Update all orders with the same hash key of the value that was just inserted
            // to recognized for orders that didn't previously have a status of 1 (Recognized by ShipSense)
            string sql = @"
	                        UPDATE o
                            SET o.ShipSenseRecognitionStatus = 1 
                            FROM [Order] o
                                INNER JOIN inserted i 
                                    ON i.Hash = o.ShipSenseHashKey
                                        AND o.ShipSenseRecognitionStatus != 1
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
