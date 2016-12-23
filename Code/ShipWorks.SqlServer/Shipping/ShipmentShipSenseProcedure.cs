using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void ShipmentShipSenseProcedure(string hashKey, SqlXml excludedShipmentXml)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = ShipmentShipSenseProcedureText;

                command.Parameters.Add(new SqlParameter("@HashKey", hashKey));
                command.Parameters.Add(new SqlParameter("@ShipmentXml", excludedShipmentXml));

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

    private const string ShipmentShipSenseProcedureText = @"
                    ------------------------------------------------------------------------------------------
                    -- Parameters passed in as parameters to the procedure
                    ------------------------------------------------------------------------------------------
                    -- @HashKey varchar(64),
                    -- @ShipmentXml XML
                    -- where the XML is in the format of
                    -- <shipment><id>XXXXXXXX</id></shipment>
                    -- <shipment><id>YYYYYYYY</id></shipment>
                    -- <shipment><id>ZZZZZZZZ</id></shipment>


                    -- Grab the entry from the knowledge base to compare with the
                    -- the entry on the shipment
                    DECLARE @Entry varbinary(max)
                    SELECT @Entry = [Entry] FROM ShipSenseKnowledgebase WITH (NOLOCK) WHERE [Hash] = @HashKey

                    -- Shread the XML into a CTE that can be used in our update
                    ;with Exclusions
                    (
                        ShipmentID
                    )
                    AS
                    (
                        SELECT
                            T.N.value('id[1]', 'varchar(10)')
                        FROM @ShipmentXml.nodes('/shipment') T(N)
                    )

                    -- Update the ShipSense status for any shipments whose
                    -- order matches the given hash key
                    UPDATE s
                    SET
                        s.ShipSenseStatus =
                            CASE
                                WHEN s.ShipSenseEntry = @Entry THEN 1
                                ELSE 2
                            end

                        FROM Shipment s
                            INNER JOIN [Order] o with(nolock)
                                ON o.OrderID = s.OrderID
                                    AND o.ShipSenseHashKey = @HashKey
                        WHERE
                            s.ShipmentID NOT IN (SELECT ShipmentID FROM Exclusions)
                            AND s.Processed = 0
                            AND s.ShipSenseStatus <> 0
                            AND s.ShipSenseStatus <>
                                CASE
                                    WHEN s.ShipSenseEntry = @Entry THEN 1
                                    ELSE 2
                                end
                            ";
}

