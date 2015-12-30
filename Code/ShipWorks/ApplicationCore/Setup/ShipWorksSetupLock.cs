using System.Data.SqlClient;
using Autofac;
using ShipWorks.Data.Utility;
using ShipWorks.SqlServer.Common.Data;

namespace ShipWorks.ApplicationCore.Setup
{
    /// <summary>
    /// Used to lock access to the ShipWorks Setup activities so only one wizard is running at a time
    /// </summary>
    public class ShipWorksSetupLock : SqlAppResourceLock
    {
        static string resourceName = "ShipWorksSetupLock";

        /// <summary>
        /// Throws a SqlAppResourceLockException if the lock cannot be taken.
        /// </summary>
        public ShipWorksSetupLock()
            : base(resourceName)
        {

        }

        /// <summary>
        /// Determines if someone current holds the ShipWorksSetupLock lock
        /// </summary>
        public static bool IsLocked()
        {
            using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
            {
                return SqlAppLockUtility.IsLocked(lifetimeScope.Resolve<SqlConnection>(), resourceName);
            }
        }
    }
}
