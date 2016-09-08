using System.Data;
using System.Data.SqlClient;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.General;

public partial class StoredProcedures
{
    /// <summary>
    /// This stored procedure deletes all FilterNodeContent records, as well as their FilterNodeContentDetail children,
    /// where the FilterNodeContent is not referenced by a FilterNode and is not being calculated.
    /// </summary>
    [SqlProcedure]
    public static void DeleteAbandonedFilterCounts()
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Ensure there are no open transactions
            UtilityFunctions.EnsureNotTransacted(con);

            int deletedCount = 0;

            // We can keep getting more batches to delete until we run out of time
            while (true)
            {
                // We'll select up to a certain amount of keys at a time - if there were tons of them, i wouldnt want this select taking too long
                SqlCommand keyCommand = con.CreateCommand();
                keyCommand.CommandText = "SELECT TOP(100) c.FilterNodeContentID FROM FilterNodeContent c WITH (READPAST) WHERE c.FilterNodeContentID NOT IN (SELECT n.FilterNodeContentID FROM FilterNode n)";

                DataTable keyTable = new DataTable();
                SqlDataAdapter adapter = new SqlDataAdapter(keyCommand);
                adapter.Fill(keyTable);

                // If we didn't get any rows, we're done
                if (keyTable.Rows.Count == 0)
                {
                    break;
                }

                foreach (DataRow row in keyTable.Rows)
                {
                    long contentID = (long) row[0];

                    SqlCommand deleteCmd = con.CreateCommand();
                    deleteCmd.CommandText = "DELETE FilterNodeContent WHERE FilterNodeContentID = @contentID";
                    deleteCmd.Parameters.AddWithValue("@contentID", contentID);
                    deleteCmd.ExecuteNonQuery();

                    deletedCount++;
                }
            }

            SqlContext.Pipe.Send("Abandoned counts deleted: " + deletedCount);
        }
    }
};
