using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;
using ShipWorks.SqlServer.Purge;
using System.Data.SqlClient;
using System.Data.SqlTypes;


public partial class StoredProcedures
{
    private const string PurgeEmailOutboundAppLockName = "PurgeEmailOutbound";

    /// <summary>
    /// Purges EmailOutbound resource data from the database and replaces it with a pointer to the dummy record.
    /// </summary>
    /// <param name="earliestRetentionDate">Indicates the date/time to use for determining
    /// which EmailOutbound records will be purged. Any records with an EmailOutbound.SentDate value earlier than
    /// this date will be purged.</param>
    /// <param name="latestExecutionTimeInUtc">This indicates the latest date/time (in UTC) that this procedure
    /// is allowed to execute. Passing in a SqlDateTime.MaxValue will effectively let the procedure run until
    /// all the appropriate records have been purged.</param>
    [SqlProcedure]
    public static void PurgeEmailOutbound(SqlDateTime earliestRetentionDateInUtc, SqlDateTime latestExecutionTimeInUtc)
    {
        using (SqlConnection connection = new SqlConnection("context connection=true"))
        {
            try
            {
                // Need to have an open connection for the duration of the lock acquisition/release
                connection.Open();

                if (!SqlAppLockUtility.IsLocked(connection, PurgeEmailOutboundAppLockName))
                {
                    if (SqlAppLockUtility.AcquireLock(connection, PurgeEmailOutboundAppLockName))
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.CommandText = PurgeEmailOutboundCommandText;

                            command.Parameters.Add(new SqlParameter("@earliestRetentionDateInUtc", earliestRetentionDateInUtc));
                            command.Parameters.Add(new SqlParameter("@latestExecutionTimeInUtc", latestExecutionTimeInUtc));
                            command.ExecuteNonQuery();

                            // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed 
                            // to the console of client (i.e. SQL Management Studio)
                            // SqlContext.Pipe.ExecuteAndSend(command);
                        }
                    }
                }
                else
                {
                    // Let the caller know that someone else is already running the purge.
                    throw new PurgeException("Could not acquire applock for purging email outbound.");
                }
            }
            finally
            {
                SqlAppLockUtility.ReleaseLock(connection, PurgeEmailOutboundAppLockName);
            }
        }
    }

    /// <summary>
    /// The T-SQL for the PurgeEmailOutbound stored procedure.
    /// </summary>
    private static string PurgeEmailOutboundCommandText
    {
        get
        {
            return @"

            ";
        }
    }
}
