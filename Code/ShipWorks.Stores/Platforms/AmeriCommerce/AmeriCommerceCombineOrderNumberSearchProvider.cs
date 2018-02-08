using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Orders.Combine.SearchProviders;

namespace ShipWorks.Stores.Platforms.AmeriCommerce
{
    /// <summary>
    /// AmeriCommerce combined order search provider
    /// </summary>
    [Component(RegisterAs = RegistrationType.Self)]
    public class AmeriCommerceCombineOrderNumberSearchProvider : CombineOrderNumberSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sqlAdapterFactory"></param>
        public AmeriCommerceCombineOrderNumberSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }
    }
}
