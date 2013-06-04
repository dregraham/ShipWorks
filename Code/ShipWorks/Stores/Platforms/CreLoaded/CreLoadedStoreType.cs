using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.UI.Wizard;
using System.Text.RegularExpressions;
using ShipWorks.Stores.Communication;
using ShipWorks.UI;
using ShipWorks.Stores.Content;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.ApplicationCore.Interaction;
using System.Windows.Forms;
using ShipWorks.Common.Threading;
using ShipWorks.Templates.Processing.TemplateXml;
using ShipWorks.Data.Grid.Paging;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.ApplicationCore.Logging;

namespace ShipWorks.Stores.Platforms.CreLoaded
{
    /// <summary>
    /// Store specific integration into ShipWorks
    /// </summary>
    class CreLoadedStoreType : GenericModuleStoreType
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public CreLoadedStoreType(StoreEntity store)
            : base(store)
        {
        }

        /// <summary>
        /// StoreType enum value
        /// </summary>
        public override StoreTypeCode TypeCode
        {
            get
            {
                return StoreTypeCode.CreLoaded;
            }
        }

        /// <summary>
        /// Log request/responses as CreLoaded
        /// </summary>
        public override ApiLogSource LogSource
        {
            get
            {
                return ApiLogSource.CreLoaded;
            }
        }
    }
}
