﻿using System;
using System.Data.Common;
using Common.Logging;
using Interapptive.Shared.Data;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Defines a single connection and transaction that should be used for data access. It's meant to replace TransactionScope
    /// for code that may take longer than 10 minutes to run, since TransactionScope has a pretty hard 10 minute timeout.
    /// </summary>
    public class ExistingConnectionScope : IDisposable
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(ExistingConnectionScope));

        /// <summary>
        /// Constructs a new SqlSessionScope.  The given SqlSession is active until
        /// the SqlSessionScope object is disposed.
        /// </summary>
        public ExistingConnectionScope()
        {
            if (ScopedConnection != null)
            {
                throw new InvalidOperationException("Only one ExistingConnectionScope can be active at a time.");
            }

            ScopedConnection = SqlSession.Current.OpenConnection();
        }

        /// <summary>
        /// Returns the sql connection that is currently in scope.  Returns null if there is no
        /// connection active.
        /// </summary>
        public static DbConnection ScopedConnection
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the sql transaction that is currently in scope.  Returns null if there is no
        /// connection active.
        /// </summary>
        public static DbTransaction ScopedTransaction
        {
            get;
            private set;
        }

        /// <summary>
        /// Commit the current transaction
        /// </summary>
        public static void Commit()
        {
            if (ScopedTransaction != null)
            {
                ScopedTransaction.Commit();
                ScopedTransaction = null;
            }
        }

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        public static void BeginTransaction()
        {
            if (ScopedTransaction != null)
            {
                throw new InvalidOperationException("Cannot start a new transaction when one has already began");
            }

            ScopedTransaction = ScopedConnection.BeginTransaction();
        }

        /// <summary>
        /// Execute the specified action with a pre-built connection and command
        /// </summary>
        public static void ExecuteWithCommand(Action<DbCommand> commandAction)
        {
            ExecuteWithCommand<object>(x =>
            {
                commandAction(x);
                return null;
            });
        }

        /// <summary>
        /// Execute the specified action with a pre-built connection and command
        /// </summary>
        public static T ExecuteWithCommand<T>(Func<DbCommand, T> commandAction)
        {
            if (ScopedConnection == null)
            {
                using (DbConnection con = SqlSession.Current.OpenConnection())
                {
                    using (DbCommand command = DbCommandProvider.Create(con))
                    {
                        return commandAction(command);
                    }
                }
            }

            using (DbCommand command = DbCommandProvider.Create(ScopedConnection))
            {
                command.Transaction = ScopedTransaction;
                return commandAction(command);
            }
        }

        /// <summary>
        /// Execute the specified action with a pre-built SqlAdapter
        /// </summary>
        public static void ExecuteWithAdapter(Action<SqlAdapter> commandAction)
        {
            ExecuteWithAdapter(false, commandAction);
        }

        /// <summary>
        /// Execute the specified action with a pre-built SqlAdapter
        /// </summary>
        public static void ExecuteWithAdapter(bool ensureTransacted, Action<SqlAdapter> commandAction)
        {
            if (ScopedConnection != null)
            {
                using (SqlAdapter adapter = new SqlAdapter(ScopedConnection, ScopedTransaction))
                {
                    commandAction(adapter);
                }
            }
            else
            {
                using (SqlAdapter adapter = new SqlAdapter(ensureTransacted))
                {
                    commandAction(adapter);
                }
            }
        }

        /// <summary>
        /// Reset the scope
        /// </summary>
        public void Dispose()
        {
            if (ScopedTransaction != null)
            {
                ScopedTransaction.Rollback();
                ScopedTransaction.Dispose();
                ScopedTransaction = null;
            }

            if (ScopedConnection != null)
            {
                ScopedConnection.Dispose();
                ScopedConnection = null;
            }

            log.InfoFormat("Leaving ExistingConnectionScope");
        }
    }
}
