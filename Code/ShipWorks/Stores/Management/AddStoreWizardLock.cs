using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Utility;
using ShipWorks.SqlServer.Common.Data;
using System.Data.SqlClient;
using ShipWorks.Data.Connection;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// Used to lock access to the AddStoreWizard so only one wizard is running at a time
    /// </summary>
    public class AddStoreWizardLock : SqlAppResourceLock
    {
        static string resourceName = "AddStoreWizard";

        /// <summary>
        /// Throws a SqlAppResourceLockException if the lock cannot be taken.
        /// </summary>
        public AddStoreWizardLock()
            : base(resourceName)
        {

        }

        /// <summary>
        /// Determins if someone current holds the AddStoreWizard lock
        /// </summary>
        public static bool IsLocked()
        {
            using (SqlConnection con = SqlSession.Current.OpenConnection())
            {
                return SqlAppLockUtility.IsLocked(con, resourceName);
            }
        }
    }
}
