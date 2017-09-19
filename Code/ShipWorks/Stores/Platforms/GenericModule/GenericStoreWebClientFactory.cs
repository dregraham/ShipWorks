using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Implementation for creating generic store web clients
    /// </summary>
    [Component]
    public class GenericStoreWebClientFactory : IGenericStoreWebClientFactory
    {
        private readonly IStoreTypeManager storeTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreWebClientFactory(IStoreTypeManager storeTypeManager)
        {
            this.storeTypeManager = storeTypeManager;
        }

        /// <summary>
        /// Create a web client for the given store
        /// </summary>
        public IGenericStoreWebClient CreateWebClient(long storeID)
        {
            IGenericModuleStoreType storeType = storeTypeManager.GetType(storeID) as IGenericModuleStoreType;

            if (storeType == null)
            {
                throw new GenericStoreException($"{storeID} is not a Generic module");
            }

            return storeType.CreateWebClient();
        }
    }
}
