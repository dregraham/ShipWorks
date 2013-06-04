using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Simple class making it possible to use a ReaderWriterLockSlim's write-lock in the context
    /// of a using statement.
    /// </summary>
    public sealed class ReaderWriterWriteLock : IDisposable
    {
        ReaderWriterLockSlim locker;

        /// <summary>
        /// Issues a EnterWriteLock on the locker.  ExitWriteLock is issued on dispose.
        /// </summary>
        public ReaderWriterWriteLock(ReaderWriterLockSlim locker)
        {
            if (locker == null)
            {
                throw new ArgumentNullException("locker");
            }

            this.locker = locker;
            this.locker.EnterWriteLock();
        }

        /// <summary>
        /// Issues a matching ExitWriteLock to the initial EnterReadLock.
        /// </summary>
        public void Dispose()
        {
            if (locker != null)
            {
                locker.ExitWriteLock();
                locker = null;
            }
        }
    }
}
