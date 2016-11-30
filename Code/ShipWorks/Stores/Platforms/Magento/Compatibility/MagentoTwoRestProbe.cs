using System;
using Autofac;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Stores.Platforms.Magento.Enums;

namespace ShipWorks.Stores.Platforms.Magento.Compatibility
{
    /// <summary>
    /// Check to see if a specific store is compatible with Magento Two Rest
    /// </summary>
    [KeyedComponent(typeof(IMagentoProbe), MagentoVersion.MagentoTwoREST)]
    public class MagentoTwoRestProbe : IMagentoProbe
    {
        /// <summary>
        /// Check to see if the store is compatible with Magento 1
        /// </summary>
        public GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store)
        {
            try
            {
                using (ILifetimeScope scope = IoC.BeginLifetimeScope())
                {
                    IMagentoTwoRestClient client =
                        scope.Resolve<IMagentoTwoRestClient>(new TypedParameter(typeof(MagentoStoreEntity), store));
                    client.GetToken();
                    return GenericResult.FromSuccess(new Uri(store.ModuleUrl));
                }
            }
            catch (Exception ex)
            {
                return GenericResult.FromError<Uri>($"An error occurred while attempting to connect to Magento. {ex.Message}");
            }
        }
    }
}