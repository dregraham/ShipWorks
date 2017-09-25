using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;

namespace ShipWorks.Stores.Platforms.SparkPay
{
    /// <summary>
    /// Combined order search provider for SparkPay
    /// </summary>
    [Component]
    public class SparkPayCombineOrderSearchProvider : CombineOrderNumberSearchProvider, ISparkPayCombineOrderSearchProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayCombineOrderSearchProvider(ISqlAdapterFactory sqlAdapterFactory) :
            base(sqlAdapterFactory)
        {

        }
    }
}