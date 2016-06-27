using System.Data.SqlClient;

namespace ShipWorks.Data.Utility
{
    /// <summary>
    /// Class used to ensure that only one running instance of ShipWorks can be doing something with a given EntityID at a time
    /// </summary>
    public sealed class SqlEntityLock : SqlAppResourceLock
    {
        long entityID;
        string reason;

        /// <summary>
        /// A lock is taken on the given entityID using the given connection, preventing any other
        /// connection also requesting a lock from working with the entityID.
        /// Throws a SqlAppResourceLockException if the lock cannot be taken.
        /// </summary>
        public SqlEntityLock(SqlConnection con, long entityID, string reason)
            : base(con, string.Format("EntityLock_{0}", entityID))
        {
            this.entityID = entityID;
            this.reason = reason;
        }

        /// <summary>
        /// A lock is taken on the given entityID, preventing any other connection also requesting a lock from working
        /// with the entityID. Throws a SqlAppResourceLockException if the lock cannot be taken.
        /// </summary>
        public SqlEntityLock(long entityID, string reason)
            : base(string.Format("EntityLock_{0}", entityID))
        {
            this.entityID = entityID;
            this.reason = reason;
        }
    }
}
