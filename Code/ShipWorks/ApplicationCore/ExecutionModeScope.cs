using System;

namespace ShipWorks.ApplicationCore
{
    /// <summary>
    /// Override the currently reported execution mode
    /// </summary>
    public class ExecutionModeScope : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ExecutionModeScope(ExecutionMode.ExecutionMode mode)
        {
            Current = mode;
        }

        /// <summary>
        /// Current execution mode
        /// </summary>
        public static ExecutionMode.ExecutionMode Current { get; private set; }

        /// <summary>
        /// Dispose the current scope
        /// </summary>
        public void Dispose()
        {
            Current = null;
        }
    }
}
