using System;

namespace ShipWorks.Actions.Tasks.Common
{
    /// <summary>
    /// Interface that will allow the actual script runner to be replaced with a mock or stub
    /// </summary>
    public interface ISqlPurgeScriptRunner
    {
        /// <summary>
        /// Gets the Utc time from the Sql server
        /// </summary>
        DateTime SqlUtcDateTime { get; }

        /// <summary>
        /// Run the specified purge script with the given parameters
        /// </summary>
        /// <param name="scriptName">Name of script resource to run</param>
        /// <param name="earliestRetentionDateInUtc">The earliest date for which data should be retained.
        /// Anything older will be purged</param>
        /// <param name="stopExecutionAfterUtc">Execution should stop after this time</param>
        void RunScript(string scriptName, DateTime earliestRetentionDateInUtc, DateTime? stopExecutionAfterUtc);

        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        void ShrinkDatabase();
    }
}