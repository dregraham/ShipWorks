using System;
using System.Threading;
using log4net;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Creates a scope within which the deadlock priority of every open connection will be set to what is configured
    /// </summary>
    public sealed class SqlDeadlockPriorityScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlDeadlockPriorityScope));

        // Only one possible at a time per thread
        static AsyncLocal<SqlDeadlockPriorityScope> activeScope = new AsyncLocal<SqlDeadlockPriorityScope>();

        /// <summary>
        /// Constructs a new SqlSessionScope.  The given SqlSession is active until
        /// the SqlSessionScope object is disposed.
        /// </summary>
        public SqlDeadlockPriorityScope(int priority)
        {
            if (activeScope.Value != null)
            {
                throw new InvalidOperationException("Only one SqlSessionScope in SqlDeadlockPriorityScope may be active at a time.");
            }

            log.InfoFormat("Entering SqlDeadlockPriorityScope ({0})", priority);

            DeadlockPriority = priority;
            activeScope.Value = this;
        }

        /// <summary>
        /// The deadlock priority of the scope
        /// </summary>
        public int DeadlockPriority { get; }

        /// <summary>
        /// Returns the SqlDeadlockPriorityScope that is currently in scope, or null if none
        /// </summary>
        public static SqlDeadlockPriorityScope Current => activeScope.Value;

        /// <summary>
        /// Leave the scope
        /// </summary>
        public void Dispose()
        {
            activeScope.Value = null;

            log.InfoFormat("Leaving SqlDeadlockPriorityScope");
        }
    }
}
