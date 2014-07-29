using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.LimeLightCRM
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class LimeLightCRMStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LimeLightCRMStoreType(StoreEntity store) :
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
                return StoreTypeCode.LimeLightCRM;
            }
        }

        /// <summary>
        /// Log request/responses as Lime Light CRM
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.LimeLightCRM;
            }
        }

        /// <summary>
        /// Gets the account settings help URL.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get
            {
                return "http://support.shipworks.com/solution/articles/4000022724-connecting-lime-light-crm-with";
            }
        }
    }
}
