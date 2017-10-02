using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Content.CombinedOrderSearchProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
