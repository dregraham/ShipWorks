using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// Specifies the Capabilities node to be generated in the output module response.
    /// </summary>
    public static class LegacyAdapterCapabilities
    {
        /// <summary>
        /// Create the base osc-derivative store capabilities value
        /// </summary>
        public static GenericModuleCapabilities CreateOscDerivativeDefaults()
        {
            return new GenericModuleCapabilities
            {
                DownloadStrategy = GenericStoreDownloadStrategy.ByModifiedTime,

                OnlineCustomerSupport = true,
                OnlineCustomerDataType = GenericVariantDataType.Numeric,

                OnlineStatusSupport = GenericOnlineStatusSupport.StatusWithComment,
                OnlineStatusDataType = GenericVariantDataType.Numeric,

                OnlineShipmentDetails = false
            };
        }

        /// <summary>
        /// Create the base xcart-derivate store capabilities value
        /// </summary>
        public static GenericModuleCapabilities CreateXCartDerivativeDefaults()
        {
            return new GenericModuleCapabilities
            {
                DownloadStrategy =  GenericStoreDownloadStrategy.ByOrderNumber,

                OnlineCustomerSupport = false,
                OnlineCustomerDataType = GenericVariantDataType.Numeric,

                OnlineStatusSupport = GenericOnlineStatusSupport.StatusOnly,
                OnlineStatusDataType = GenericVariantDataType.Text,

                OnlineShipmentDetails = true
            };
        }
    }
}
