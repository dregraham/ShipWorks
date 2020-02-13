﻿using System;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ShipWorks.Data.Connection
{
    /// <summary>
    /// Factory for creating SqlAdapters
    /// </summary>
    public interface ISqlAdapterFactory
    {
        /// <summary>
        /// Create a SqlAdapter that is not part of a transaction
        /// </summary>
        ISqlAdapter Create();

        /// <summary>
        /// Create a SqlAdapter that IS part of a transaction
        /// </summary>
        ISqlAdapter CreateTransacted();

        /// <summary>
        /// Execute a block of code using a SqlAdapter that is part of a physical transaction
        /// </summary>
        Task<T> WithPhysicalTransactionAsync<T>(Func<ISqlAdapter, Task<T>> withAdapter, [CallerMemberName] string name = "");

        /// <summary>
        /// Execute a block of code using a SqlAdapter that is part of a physical transaction
        /// </summary>
        void WithPhysicalTransaction(Action<ISqlAdapter> withAdapter, [CallerMemberName] string name = "");

        /// <summary>
        /// Create a SqlAdatper that uses an existing connection and transaction
        /// </summary>
        ISqlAdapter Create(DbConnection connection, DbTransaction transaction);

        /// <summary>
        /// Create a SqlAdatper that uses an existing connection
        /// </summary>
        ISqlAdapter Create(DbConnection connection);
    }
}
