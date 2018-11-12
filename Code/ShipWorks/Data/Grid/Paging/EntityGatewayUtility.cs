using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ShipWorks.Data.Grid.Paging
{
    /// <summary>
    /// Utilities for entity gateways
    /// </summary>
    public static class EntityGatewayUtility
    {
        /// <summary>
        /// Execute a function that can time out
        /// </summary>
        public static T ExecuteWithTimeout<T>(TimeSpan? timeout, Stopwatch timer, Func<T> func)
        {
            if (timeout == null)
            {
                return func();
            }

            TimeSpan timeRemaining = timeout.Value - timer.Elapsed;

            // Only bother if there is still time remaining
            if (timeRemaining > TimeSpan.Zero)
            {
                // Kickoff the background task
                var task = Task.Factory.StartNew(() =>
                {
                    return func();
                });

                // SpinWait until its completed, or until the timeout expires.  Don't use events here, b\c they pump (due to COM STA), which
                // can then make this re-entrant.
                SpinWait.SpinUntil(() => task.IsCompleted || (timeout - timer.Elapsed) < TimeSpan.Zero);

                // If it actually finished, grab the results
                if (task.IsCompleted)
                {
                    return task.Result;
                }
            }

            return default(T);
        }
    }
}
