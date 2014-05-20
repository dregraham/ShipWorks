using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Creates a scope within wich the deadlock priorty of every open connection will be set to what is configured
    /// </summary>
    public sealed class SqlDeadlockPriorityScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(SqlDeadlockPriorityScope));

        // Only one possible at a time per thread
        [ThreadStatic]
        static SqlDeadlockPriorityScope activeScope;

        // The deadlock priority to set
        int priority;

        /// <summary>
        /// Constructs a new SqlSessionScope.  The given SqlSession is active until
        /// the SqlSessionScope object is disposed.
        /// </summary>
        public SqlDeadlockPriorityScope(int priority)
        {
            if (SqlDeadlockPriorityScope.activeScope != null)
            {
                throw new InvalidOperationException("Only one SqlSessionScope in SqlDeadlockPriorityScope may be active at a time.");
            }

            log.InfoFormat("Entering SqlDeadlockPriorityScope ({0})", priority);

            this.priority = priority;
            SqlDeadlockPriorityScope.activeScope = this;
        }

        /// <summary>
        /// The deadlock priority of the scope
        /// </summary>
        public int DeadlockPriority
        {
            get { return priority; }
        }

        /// <summary>
        /// Returns the SqlDeadlockPriorityScope that is currently in scope, or null if none
        /// </summary>
        public static SqlDeadlockPriorityScope Current
        {
            get { return activeScope; }
        }

        /// <summary>
        /// Leave the scope
        /// </summary>
        public void Dispose()
        {
            SqlDeadlockPriorityScope.activeScope = null;

            log.InfoFormat("Leaving SqlDeadlockPriorityScope");
        }    
    }
}
