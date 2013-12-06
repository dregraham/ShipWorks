using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores;

namespace ShipWorks.ApplicationCore.Setup
{
    /// <summary>
    /// Utility functions for dealing with what parts of ShipWorks setup need done
    /// </summary>
    public static class ShipWorksSetupManager
    {
        /// <summary>
        /// Indicates if all pieces that we consider essentially to a succesfull initial setup are complete
        /// </summary>
        public static bool IsFullySetup()
        {
            // First, there has to be a store
            if (StoreManager.GetDatabaseStoreCount() == 0)
            {
                return false;
            }

            return true;
        }
    }
}
