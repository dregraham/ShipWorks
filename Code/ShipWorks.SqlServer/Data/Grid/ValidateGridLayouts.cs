using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;


public partial class StoredProcedures
{
    [SqlProcedure]
    public static void ValidateGridLayouts()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            SqlCommand cmd = con.CreateCommand();
            cmd.CommandText = @"
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;

                DECLARE @offending int;

                WITH Grouped AS
                (
                   SELECT GridColumnLayoutID,
                          COUNT(*) AS Positions, 
                          COUNT(DISTINCT Position) AS UniquePositions, 
                          CAST(COUNT(*) * ((COUNT(*) - 1) / 2.0) AS int) AS TargetSum, 
                          SUM(Position) AS ActualSum
                   FROM GridColumnPosition
                   GROUP BY GridColumnLayoutID
                )   
                SELECT @offending = COUNT(GridColumnLayoutID)
                  FROM Grouped g
                  WHERE g.Positions != g.UniquePositions OR g.TargetSum != g.ActualSum

                if (@offending > 0)
                    RAISERROR('The grid layout is corrupt.', 16, 1);
            ";

            cmd.ExecuteNonQuery();
        }
    }
}