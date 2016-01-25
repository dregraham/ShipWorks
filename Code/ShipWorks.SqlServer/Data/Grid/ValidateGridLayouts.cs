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

            DECLARE @BrokenGridColumnLayout TABLE
            (
	            GridColumnLayoutID int
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

            INSERT INTO @BrokenGridColumnLayout (GridColumnLayoutID)
            SELECT GridColumnLayoutID 
	            FROM Grouped g
	            WHERE g.Positions != g.UniquePositions OR g.TargetSum != g.ActualSum;

	            BEGIN TRY 
		            WHILE (SELECT COUNT(*) FROM @BrokenGridColumnLayout) > 0
			            BEGIN

				            DECLARE @OffendingId int;
				            SELECT @OffendingId = (SELECT TOP(1) GridColumnLayoutID FROM @BrokenGridColumnLayout);

					            DECLARE @GridColumnPositionId int
					            DECLARE @Position int
					            SELECT @Position = 0

					            DECLARE MY_CURSOR CURSOR 
						            LOCAL STATIC READ_ONLY FORWARD_ONLY
					            FOR 
					            SELECT GridColumnPositionID 
						            FROM GridColumnPosition
						            WHERE GridColumnLayoutID = @OffendingId
						            ORDER BY Position;

					            OPEN MY_CURSOR
					            FETCH NEXT FROM MY_CURSOR INTO @GridColumnPositionId
					            WHILE @@FETCH_STATUS = 0
					            BEGIN 
						            UPDATE GridColumnPosition SET Position = @Position WHERE GridColumnPositionID = @GridColumnPositionId
						            SELECT @Position = @Position + 1
						            FETCH NEXT FROM MY_CURSOR INTO @GridColumnPositionId
					            END
					            CLOSE MY_CURSOR
					            DEALLOCATE MY_CURSOR

				            DELETE FROM @BrokenGridColumnLayout WHERE GridColumnLayoutID = @OffendingId;
			            END
	            END TRY
	            BEGIN CATCH 
		            RAISERROR('The grid layout is corrupt.', 16, 1);
	            END CATCH;
            ";

            cmd.ExecuteNonQuery();
        }
    }
}