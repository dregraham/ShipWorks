using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.Diagnostics;
using System.Threading;
using ShipWorks.SqlServer.Filters;
using ShipWorks.Filters;
using ShipWorks.SqlServer.General;

public partial class StoredProcedures
{
    [SqlProcedure]
    public static void CalculateInitialFilterCounts(out SqlInt32 nodesUpdated)
    {
        // Attach to the connection
        using (SqlConnection con = new SqlConnection("Context connection = true"))
        {
            con.Open();

            // Cannot be in a transaction on entry
            UtilityFunctions.EnsureNotTransacted(con);

            long nodeID;
            long countID;
            string calculation;

            int countsTaken = 0;
            nodesUpdated = 0;

            TimeSpan timeLimit = TimeSpan.FromSeconds(15);

            // Do as many as possible within the time limit.  If we didn't limit this, then if new counts just keep being created constantly (not sure why they would),
            // but then we'd never leave this function, and either block someone else needing to do counts, or SQL Timeout
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                // Keep going until we run out of time
                while (stopwatch.Elapsed < timeLimit)
                {
                    // Wait up to 5 seconds in case someone else is currently using the lock
                    if (!ActiveCalculationUtility.AcquireCalculatingLock(con, TimeSpan.FromSeconds(5)))
                    {
                        SqlContext.Pipe.Send("CalculateInitialFilterCounts: could not obtain lock.");
                        return;
                    }

                    try
                    {
                        if (!GetNextCountToCalculate(con, out nodeID, out countID, out calculation))
                        {
                            return;
                        }

                        // The calculation string is designed to take a single replacement value of the FilterNodeID. Didn't use {0} notation though
                        // since a user could have that in their actual filter content
                        calculation = calculation.Replace("<SwFilterNodeID />", nodeID.ToString());

                        // Perform the count
                        SqlCommand calculationCmd = con.CreateCommand();
                        calculationCmd.CommandText = calculation;
                        calculationCmd.ExecuteNonQuery();

                        countsTaken++;
                    }
                    finally
                    {
                        ActiveCalculationUtility.ReleaseCalculatingLock(con);
                        nodesUpdated = countsTaken;
                    }
                }
            }
            finally
            {
                SqlContext.Pipe.Send(string.Format("InitialCounts complete ({0})", countsTaken));
            }
        }
    }

    /// <summary>
    /// Get the next count to be calculated
    /// </summary>
    private static bool GetNextCountToCalculate(SqlConnection con, out long nodeID, out long countID, out string calculation)
    {
        nodeID = 0;
        countID = 0;
        calculation = null;

        // Look for all counts that are still in their initial state.  We do all filters first, then folders.  Filter
        // counts have to be done before folder counts for folder counts to be accurate.
        //
        // We check for counts that need initial still, or *think* the are runnint there initial counts.  Any counts that
        // are marked as currently runnning initial counts have actualy _failed_ to run initial counts.  B\c we cannot
        // be in this routine from two places at once.  So we know someone else isn't running it - it must be marked as running
        // b\c it failed and never get marked as not running.
        //
        // We also do not do nodes for Search - the initial count for searches is handled by the search engine.
        //
        SqlCommand cmd = con.CreateCommand();
        cmd.CommandText = string.Format(@"
            WITH NextCount AS
            (
	            SELECT TOP(1) n.FilterNodeID, c.FilterNodeContentID, c.InitialCalculation, c.Status
	              FROM FilterNode n INNER JOIN FilterSequence s ON n.FilterSequenceID = s.FilterSequenceID 
						            INNER JOIN Filter f ON s.FilterID = f.FilterID 
						            INNER JOIN FilterNodeContent c ON n.FilterNodeContentID = c.FilterNodeContentID
                  WHERE (Status = {0} OR Status = {1}) AND n.Purpose != {2}
	              ORDER BY f.IsFolder ASC
            )
            UPDATE NextCount
               SET Status = {1}
               OUTPUT deleted.FilterNodeID, inserted.FilterNodeContentID, inserted.InitialCalculation, deleted.Status
            ",
            (int) FilterCountStatus.NeedsInitialCount,
            (int) FilterCountStatus.RunningInitialCount,
            (int) FilterNodePurpose.Search);

        bool hasResult = false;

        // See if there is a count to take
        using (SqlDataReader reader = cmd.ExecuteReader())
        {
            if (reader.Read())
            {
                nodeID = reader.GetInt64(0);
                countID = reader.GetInt64(1);
                calculation = reader.GetString(2);

                // See what the status was.
                int oldStatus = Convert.ToInt32(reader[3]);
                if (oldStatus == (int) FilterCountStatus.RunningInitialCount)
                {
                    SqlContext.Pipe.Send(string.Format("Redoing initial count {0} - no active calculation found.", countID));
                }

                hasResult = true;
            }
        }

        return hasResult;
    }
};
