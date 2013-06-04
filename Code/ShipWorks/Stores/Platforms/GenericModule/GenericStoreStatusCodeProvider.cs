using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;
using System.Xml.XPath;
using ShipWorks.Data;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Connection;
using ShipWorks.Stores.Platforms.GenericModule;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Manages the status code to description mappings for an GenericStore store.
    /// </summary>
    public class GenericStoreStatusCodeProvider : OnlineStatusCodeProvider<object>
    {
        // Logger
        static readonly ILog log = LogManager.GetLogger(typeof(GenericStoreStatusCodeProvider));

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreStatusCodeProvider(GenericModuleStoreEntity store)
            : base(store, GenericModuleStoreFields.ModuleStatusCodes)
        {

        }

        /// <summary>
        /// Convert the given value in it's stored representation to the dataType specified by the class
        /// </summary>
        public override object ConvertCodeValue(object value)
        {
            if (((GenericModuleStoreEntity) Store).ModuleOnlineStatusDataType == (int) GenericVariantDataType.Numeric)
            {
                return Convert.ToInt64(value);
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Retrieves the available online status codes from the store.
        /// </summary>
        protected override Dictionary<object, string> GetCodeMapFromOnline()
        {
            try
            {
                GenericModuleStoreType storeType = (GenericModuleStoreType) StoreTypeManager.GetType(Store);

                // Create and initialize the web client 
                GenericStoreWebClient webClient = storeType.CreateWebClient();

                // execute the request to retrieve statuses
                GenericModuleResponse response = webClient.GetStatusCodes();

                // Generic store uses the same xml format as we store locally, so load using base functionality
                return DeserializeCodeMapFromXml(response.XPath);
            }
            catch (GenericStoreException ex)
            {
                log.ErrorFormat("ShipWorks was unable to retrieve available status codes from the online store: {0}", ex.Message);

                // return null skip import
                return null;
            }
        }
    }
}
