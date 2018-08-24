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
        /// <param name="olderThanInUtc">The earliest date for which data should be retained.
        /// Anything older will be purged</param>
        /// <param name="runUntilInUtc">Execution should stop after this time</param>
        /// <param name="retryAttempts">Number of times to retry the purge if a handleable error is detected.  Pass 0 to not retry.</param>
        /// <param name="softDelete">If true, resources/object references will be pointed to dummy entities.  Otherwise the full entity will be deleted.</param>
        void RunScript(string scriptName, DateTime olderThanInUtc, DateTime? runUntilInUtc, int retryAttempts, bool softDelete);

        /// <summary>
        /// Connects to the database and attempts to shrink the database.
        /// </summary>
        void ShrinkDatabase();
    }
}