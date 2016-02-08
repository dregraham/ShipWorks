using System.Data.SqlClient;
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

                DECLARE @GridColumnLayoutID int;

                DECLARE @BrokenGridColumnLayout TABLE
                (
	                GridColumnLayoutID int
                );

                DECLARE @WorkingGridColumnLayout TABLE
                (
	                GridColumnLayoutID int,
	                GridColumnPositionID int,
	                NewPosition int
                );

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

                --Populate a table of all the GridColumnLayoutIDs that are broken
                INSERT INTO @BrokenGridColumnLayout (GridColumnLayoutID)
	                SELECT DISTINCT GridColumnLayoutID
	                FROM Grouped g
	                WHERE g.Positions != g.UniquePositions OR g.TargetSum != g.ActualSum

                -- While there are broken rows
                WHILE (SELECT COUNT(*) FROM @BrokenGridColumnLayout) > 0
                BEGIN
	                -- The ID we are going to fix 
	                SELECT @GridColumnLayoutID = (SELECT TOP(1) GridColumnLayoutID FROM @BrokenGridColumnLayout)

	                -- Populate the @WorkingGridColumnLayout table with all of the GridColumnPositionIDs and new Positions
	                INSERT INTO @WorkingGridColumnLayout (GridColumnLayoutID, GridColumnPositionID, NewPosition)
		                SELECT GridColumnLayoutID, GridColumnPositionID ,ROW_NUMBER() OVER (ORDER BY Position) - 1
		                FROM GridColumnPosition WHERE GridColumnLayoutID = @GridColumnLayoutID
	
	                -- Update the GridColumnPosition table with the new values from the @WorkingGridColumnLayout table 
	                UPDATE GridColumnPosition 
		                SET Position = wgcl.NewPosition 
		                FROM GridColumnPosition gcp 
		                INNER JOIN @WorkingGridColumnLayout wgcl 
		                ON gcp.GridColumnPositionID = wgcl.GridColumnPositionID

	                -- Remove the id once its fixed
	                DELETE FROM @BrokenGridColumnLayout 
		                WHERE GridColumnLayoutID = @GridColumnLayoutID

	                DELETE FROM @WorkingGridColumnLayout
                END
            ";

            cmd.ExecuteNonQuery();
        }
    }
}