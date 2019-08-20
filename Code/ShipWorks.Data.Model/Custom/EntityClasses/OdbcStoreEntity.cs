using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShipWorks.Data.Model.EntityClasses
{
    /// <summary>
    /// Custom OdbcStoreEntity
    /// </summary>
    public partial class OdbcStoreEntity 
    {
        /// <summary>
        /// True if the user is setting up a store they downloaded from the Hub.
        /// </summary>
        public bool InExistingHubStoreSetup => WarehouseStoreID.HasValue && !SetupComplete;
    }
}
