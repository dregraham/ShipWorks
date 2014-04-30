using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [Microsoft.SqlServer.Server.SqlProcedure]
    public static void ResetShipSense()
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = @"
                    UPDATE [ORDER] SET ShipSenseRecognitionStatus = 0

                    TRUNCATE TABLE ShipSenseKnowledgebase";

                connection.Open();

                // Use ExecuteAndSend instead of ExecuteNonQuery when debugging to see output printed 
                // to the console of client (i.e. SQL Management Studio)
                if (SqlContext.Pipe != null)
                {
                    SqlContext.Pipe.ExecuteAndSend(command);
                }
                else
                {
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }
    }
}
