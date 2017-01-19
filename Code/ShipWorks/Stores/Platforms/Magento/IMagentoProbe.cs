using System;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Interface that represents a probe of a Magento store for compatibility
    /// </summary>
    public interface IMagentoProbe
    {
        /// <summary>
        /// Probe the given Magento store to see if it is compatible
        /// </summary>
        GenericResult<Uri> FindCompatibleUrl(MagentoStoreEntity store);
    }
}