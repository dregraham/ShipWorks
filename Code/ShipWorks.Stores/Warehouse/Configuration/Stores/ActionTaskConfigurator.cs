using System.Collections.Generic;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Actions;
using ShipWorks.Actions.Tasks;
using ShipWorks.Actions.Triggers;
using ShipWorks.Data.Connection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Platform.CoreExtensions.Actions;
using ShipWorks.Warehouse.Configuration.Stores;
using ShipWorks.Warehouse.Configuration.Stores.DTO;

namespace ShipWorks.Stores.Warehouse.Configuration.Stores
{
    /// <summary>
    /// Creates an action task for stores managed in the hub
    /// </summary>
    [Component]
    public class ActionTaskConfigurator : IActionTaskConfigurator
    {
        private readonly ISqlAdapterFactory sqlAdapterFactory;
        private readonly IActionTaskFactory actionTaskFactory;
        private readonly IActionManager actionManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ActionTaskConfigurator(ISqlAdapterFactory sqlAdapterFactory, IActionTaskFactory actionTaskFactory, IActionManager actionManager)
        {
            this.sqlAdapterFactory = sqlAdapterFactory;
            this.actionTaskFactory = actionTaskFactory;
            this.actionManager = actionManager;
        }

        /// <summary>
        /// Configure the Action Task if required
        /// </summary>
        public void Configure(StoreConfiguration configuration, StoreEntity store, bool isNew)
        {
            if (!isNew || !configuration.ManagedInHub || !configuration.UploadShipmentDetails)
            {
                // only creating actions if it is a new store, managedInHub and the user chose to UploadShipmentDetails
                return;
            }

            // Setup the trigger.  We know ShipmentProcessedTrigger doesn't save extra state to the DB, so we don't need to call that function.
            ShipmentProcessedTrigger trigger = new ShipmentProcessedTrigger();

            // Setup the basic action
            var action = new ActionEntity
            {
                Name = "Store Update",
                Enabled = true,
                ComputerLimitedType = (int) ComputerLimitedType.TriggeringComputer,
                InternalComputerLimitedList = string.Empty,
                StoreLimited = true,
                StoreLimitedList = new long[] {store.StoreID},
                TriggerType = (int) trigger.TriggerType,
                TriggerSettings = trigger.GetXml()
            };

            var actionTask = actionTaskFactory.Create(typeof(PlatformShipmentUploadTask), store, 0);

            // Set the summary
            action.TaskSummary = actionManager.GetTaskSummary(new List<ActionTask> {actionTask});

            using (var sqlAdapter = sqlAdapterFactory.Create())
            {
                actionManager.SaveAction(action, sqlAdapter);
                actionTask.Save(action, sqlAdapter);
                sqlAdapter.Commit();
            }
        }
    }
}