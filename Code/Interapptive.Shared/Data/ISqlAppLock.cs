using System;

namespace Interapptive.Shared.Data
{
    /// <summary>
    /// Interface for SQL App Lock class
    /// </summary>
    public interface ISqlAppLock : IDisposable
    {
        bool LockAcquired { get; set; }
    }
}