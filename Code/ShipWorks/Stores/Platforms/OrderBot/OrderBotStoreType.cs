using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.OrderBot
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class OrderBotStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OrderBotStoreType(StoreEntity store) :
            base(store)
        {

        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.OrderBot;
            }
        }

        /// <summary>
        /// Log request/responses as OrderBot
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.OrderBot;
            }
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/"; }
        }
    }
}
