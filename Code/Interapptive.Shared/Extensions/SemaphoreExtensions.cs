using System;
using System.Reactive.Disposables;
using System.Threading;

namespace Interapptive.Shared.Extensions
{
    /// <summary>
    /// Extensions for Semaphores
    /// </summary>
    public static class SemaphoreExtensions
    {
        /// <summary>
        /// Wait on a semaphore, releasing the wait when the return value is disposed
        /// </summary>
        public static IDisposable DisposableWait(this SemaphoreSlim semaphore)
        {
            semaphore.Wait();
            return Disposable.Create(() => semaphore.Release());
        }
    }
}
