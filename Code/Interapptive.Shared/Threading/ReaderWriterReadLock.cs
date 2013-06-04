using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Interapptive.Shared.Threading
{
    /// <summary>
    /// Simple class making it possible to use a ReaderWriterLockSlim's read-lock in the context
    /// of a using statement.
    /// </summary>
    public sealed class ReaderWriterReadLock : IDisposable
    {
        ReaderWriterLockSlim locker;

        /// <summary>
        /// Issues a EnterReadLock on the locker.  ExitReadLock is issued on dispose.
        /// </summary>
        public ReaderWriterReadLock(ReaderWriterLockSlim locker)
        {
            if (locker == null)
            {
                throw new ArgumentNullException("locker");
            }

            this.locker = locker;
            this.locker.EnterReadLock();
        }

        /// <summary>
        /// Issues a matching ExitReadLock to the initial EnterReadLock.
        /// </summary>
        public void Dispose()
        {
            if (locker != null)
            {
                locker.ExitReadLock();
                locker = null;
            }
        }
    }
}
