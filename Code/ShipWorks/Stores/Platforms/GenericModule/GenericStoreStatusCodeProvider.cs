using System;
using System.Collections.Generic;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using log4net;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Manages the status code to description mappings for an GenericStore store.
    /// </summary>
    public class GenericStoreStatusCodeProvider : OnlineStatusCodeProvider<object>
    {
        // Loggers - these will be the same at run time, but the instance version can be mocked out during testing
        static readonly ILog log = LogManager.GetLogger(typeof(GenericStoreStatusCodeProvider));
        private readonly ILog logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreStatusCodeProvider(GenericModuleStoreEntity store)
            : this(store, log)
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreStatusCodeProvider(GenericModuleStoreEntity store, ILog logger)
            : base(store, GenericModuleStoreFields.ModuleStatusCodes)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Convert the given value in it's stored representation to the dataType specified by the class
        /// </summary>
        public override object ConvertCodeValue(object value)
        {
            return StoreUsesNumericStatusCodes ? 
                ConvertToNumeric(value) : 
                ConvertToText(value);
        }

        /// <summary>
        /// Tests whether the specified code is valid
        /// </summary>
        public override bool IsValidCode(object code)
        {
            if (code == null)
            {
                return false;
            }

            if (StoreUsesNumericStatusCodes && (code is long || code is int)) 
            {
                return true;
            }

            if (!StoreUsesNumericStatusCodes && code is string)
            {
                return true;
            }

            return false;
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
                logger.ErrorFormat("ShipWorks was unable to retrieve available status codes from the online store: {0}", ex.Message);

                // return null skip import
                return null;
            }
        }

        /// <summary>
        /// Gets whether the store uses numeric status codes
        /// </summary>
        private bool StoreUsesNumericStatusCodes
        {
            get
            {
                return ((GenericModuleStoreEntity)Store).ModuleOnlineStatusDataType == (int)GenericVariantDataType.Numeric;
            }
        }

        /// <summary>
        /// Convert the given value to text
        /// </summary>
        private object ConvertToText(object value)
        {
            if (value != null)
            {
                return value.ToString();
            }

            logger.Warn("Could not null status code");
            return null;
        }

        /// <summary>
        /// Converts the given value to an Int64
        /// </summary>
        private object ConvertToNumeric(object value)
        {
            try
            {
                return Convert.ToInt64(value);
            }
            catch (FormatException ex)
            {
                logger.Warn(string.Format("Could not parse status code [{0}] into a number", value), ex);
                return null;
            }
        }
    }
}
