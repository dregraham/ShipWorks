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
	                        UPDATE o
                            SET o.ShipSensible = 1 
                            FROM [Order] o
                                INNER JOIN inserted i 
                                    ON i.Hash = o.ShipSenseHashKey
                                        AND o.ShipSensible = 0
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
