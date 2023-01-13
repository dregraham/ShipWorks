using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Tasks.Common;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Etsy.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Etsy.CoreExtensions.Actions
{
    /// <summary>
    /// Task for uploading shipment details to Etsy
    /// </summary>
    [ActionTask("Update store status", "EtsyUploadTask", ActionTaskCategory.UpdateOnline)]
    public class EtsyUploadTask : StoreInstanceTaskBase
    {
        private const string errorMessage = "ShipWorks no loner supports marking items as shipped or paid. Use \"Upload Shipment Details.\"";

        /// <summary>
        /// Constructor
        /// </summary>
        public EtsyUploadTask()
        {
        }

        /// <summary>
        /// Descriptive label which appears on the task editor
        /// </summary>
        public override string InputLabel => "Set status of:";

        /// <summary>
        /// Indicates if the task is supported for the specified store
        /// </summary>
        public override bool SupportsStore(StoreEntity store) => false;

        /// <summary>
        /// Instantiates the editor for the action
        /// </summary>
        public override ActionTaskEditor CreateEditor() => new DeprecatedTaskEditor(errorMessage);

        /// <summary>
        /// Executes the task
        /// </summary>
        public override void Run(List<long> inputKeys, ActionStepContext context)
        {
            throw new ActionTaskRunException(errorMessage);
        }
    }
}