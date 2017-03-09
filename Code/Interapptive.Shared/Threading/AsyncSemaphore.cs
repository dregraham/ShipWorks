using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Semaphore that can be used with async methods
    /// </summary>
    /// <remarks>
    /// This is from https://blogs.msdn.microsoft.com/pfxteam/2012/02/12/building-async-coordination-primitives-part-5-asyncsemaphore/
    /// and is only necessary because .NET 4 does not have any async wait methods.  .NET 4.5 has WaitAsync, which
    /// we can use when we upgrade eventually.</remarks>
    public class AsyncSemaphore
    {
        private readonly static Task<bool> completedTask = Task.FromResult(true);
        private readonly Queue<TaskCompletionSource<bool>> waiters = new Queue<TaskCompletionSource<bool>>();
        private int currentCount;

        /// <summary>
        /// Constructor
        /// </summary>
        public AsyncSemaphore(int initialCount)
        {
            if (initialCount < 0)
            {
                throw new ArgumentOutOfRangeException("initialCount");
            }

            currentCount = initialCount;
        }

        /// <summary>
        /// Wait for the lock to become available
        /// </summary>
        public Task<bool> WaitAsync() => WaitAsync(Timeout.Infinite);

        public Task<bool> WaitAsync(TimeSpan timeout) => WaitAsync((int) timeout.TotalMilliseconds);

        /// <summary>
        /// Wait for the lock to become available
        /// </summary>
        public Task<bool> WaitAsync(int timeout)
        {
            lock (waiters)
            {
                if (currentCount > 0)
                {
                    --currentCount;
                    return completedTask;
                }
                else
                {
                    var waiter = new TaskCompletionSource<bool>();
                    waiters.Enqueue(waiter);

                    return timeout > 0 ?
                        CreateTimeoutTask(timeout, waiter.Task) :
                        waiter.Task;
                }
            }
        }

        /// <summary>
        /// Create a waiter that will timeout eventually
        /// </summary>
        private Task<bool> CreateTimeoutTask(int timeout, Task<bool> waiter)
        {
            var delayer = Task.Delay(timeout).ContinueWith(x => false);

            return Task.WhenAny(waiter, delayer)
                .ContinueWith(x => x.Result.Result);
        }

        /// <summary>
        /// Release the given lock
        /// </summary>
        public void Release()
        {
            TaskCompletionSource<bool> toRelease = null;

            lock (waiters)
            {
                if (waiters.Count > 0)
                {
                    toRelease = waiters.Dequeue();
                }
                else
                {
                    ++currentCount;
                }
            }

            if (toRelease != null)
            {
                toRelease.SetResult(true);
            }
        }
    }
}
