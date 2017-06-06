using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Factory to get resource locks
    /// </summary>
    [Component]
    public class ResourceLockFactory : IResourceLockFactory
    {
        /// <summary>
        /// Get an entity lock, if possible
        /// </summary>
        /// <remarks>Throws SqlAppResourceLockException if lock couldn't be secured</remarks>
        public IDisposable GetEntityLock(long entityID, string reason) => new SqlEntityLock(entityID, reason);
    }
}
