using System;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Factory to get resource locks
    /// </summary>
    public interface IResourceLockFactory
    {
        /// <summary>
        /// Get an entity lock, if possible
        /// </summary>
        /// <remarks>Throws SqlAppResourceLockException if lock couldn't be secured</remarks>
        IDisposable GetEntityLock(long entityID, string reason);
    }
}
