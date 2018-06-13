using System;
using System.Reactive;
using System.Threading.Tasks;
using ShipWorks.Data.Connection;
using static Interapptive.Shared.Utility.Functional;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Functions for working with data
    /// </summary>
    public static class DataFunctions
    {
        /// <summary>
        /// Execute a method with the given deadlock priority
        /// </summary>
        public static Unit WithDeadlockPriority(int deadlockPriority, Func<Unit> method) =>
            Using(
                new SqlDeadlockPriorityScope(deadlockPriority),
                _ => method());

        /// <summary>
        /// Execute a method with the given sql adapter
        /// </summary>
        public static Unit WithSqlAdapter(Func<ISqlAdapter> createSqlAdapter, Func<ISqlAdapter, Unit> method) =>
            Using(
                createSqlAdapter(),
                (ISqlAdapter sqlAdapter) =>
                {
                    sqlAdapter.CommandTimeOut = (int) TimeSpan.FromMinutes(10).TotalSeconds;

                    method(sqlAdapter);

                    sqlAdapter.Commit();

                    return Unit.Default;
                });

        /// <summary>
        /// Execute a method with the given deadlock priority
        /// </summary>
        public static async Task<T> WithDeadlockPriorityAsync<T>(int deadlockPriority, Func<Task<T>> method) =>
            await UsingAsync(
                new SqlDeadlockPriorityScope(deadlockPriority),
                _ => method());

        /// <summary>
        /// Execute a method with the given sql adapter
        /// </summary>
        public static Task<T> WithSqlAdapterAsync<T>(Func<ISqlAdapter> createSqlAdapter, Func<ISqlAdapter, Task<T>> method) =>
            UsingAsync(
                createSqlAdapter(),
                async (ISqlAdapter sqlAdapter) =>
                {
                    sqlAdapter.CommandTimeOut = (int) TimeSpan.FromMinutes(10).TotalSeconds;

                    var result = await method(sqlAdapter).ConfigureAwait(false);

                    sqlAdapter.Commit();

                    return result;
                });
    }
}
