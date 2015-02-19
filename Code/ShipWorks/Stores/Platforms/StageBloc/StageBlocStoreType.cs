using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericModule;

namespace ShipWorks.Stores.Platforms.StageBloc
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    public class StageBlocStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public StageBlocStoreType(StoreEntity store) :
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
                return StoreTypeCode.StageBloc;
            }
        }

        /// <summary>
        /// Log request/responses as StageBloc
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.StageBloc;
            }
        }

        /// <summary>
        /// Gets the help URL to use in the account settings.
        /// </summary>
        public override string AccountSettingsHelpUrl
        {
            get { return "http://support.shipworks.com/support/solutions/articles/4000045732-connecting-stagebloc-with-shipworks"; }
        }
    }
}
