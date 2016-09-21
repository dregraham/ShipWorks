using System.Data.SqlClient;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.SqlServer.Purge
{
    /// <summary>
    /// Runs Purge Scripts
    /// </summary>
    public static class PurgeScriptRunner
    {
        public const string OlderThanParameterName = "@olderThan";

        public const string RunUntilParameterName = "@runUntil";

        /// <summary>
        /// Runs a sql script. Assumes script has paramaters @olderThan and @runUntil.
        /// </summary>
        /// <param name="script">Sql script to run. Assumes script has paramaters @olderThan and @runUntil.</param>
        /// <param name="purgeAppLockName">Checks for this AppLock. If applock is taken, script will not run and purge exception is thrown.</param>
        /// <param name="olderThan">Delete records older than this date</param>
        /// <param name="runUntil">Stop running the script if it runs longer than this time</param>
        public static void RunPurgeScript(string script, string purgeAppLockName, SqlDateTime olderThan, SqlDateTime runUntil)
        {
            using (SqlConnection connection = new SqlConnection("context connection=true"))
            {
                SqlAppLockUtility.RunLockedCommand(connection, purgeAppLockName, com =>
                {
                    SqlCommand command = (SqlCommand) com;

                    command.CommandText = script;

                    command.Parameters.Add(new SqlParameter(OlderThanParameterName, olderThan));
                    command.Parameters.Add(new SqlParameter(RunUntilParameterName, runUntil));

                    // Use ExecuteAndSend instead of ExecuteNonQuery when debuggging to see output printed
                    // to the console of client (i.e. SQL Management Studio)
                    if (SqlContext.Pipe != null)
                    {
                        SqlContext.Pipe.ExecuteAndSend(command);
                    }
                    else
                    {
                        command.ExecuteNonQuery();
                    }
                });
            }
        }
    }
}