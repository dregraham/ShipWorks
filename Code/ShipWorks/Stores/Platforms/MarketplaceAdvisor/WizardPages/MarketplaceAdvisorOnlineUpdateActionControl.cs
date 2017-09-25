using System.Collections.Generic;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.MarketplaceAdvisor.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.MarketplaceAdvisor.WizardPages
{
    /// <summary>
    /// Control for creating online update action tasks for the MA add store wizard
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
    public partial class MarketplaceAdvisorOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MarketplaceAdvisorOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the UI based on the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            MarketplaceAdvisorStoreEntity maStore = (MarketplaceAdvisorStoreEntity) store;

            // Order workflow\promotion only applies to OMS
            promote.Visible = (maStore.AccountType == (int) MarketplaceAdvisorAccountType.OMS);
        }

        /// <summary>
        /// Create tasks for the given store based on user selection
        /// </summary>
        public override List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            List<ActionTask> tasks = new List<ActionTask>();

            if (shipmentUpdate.Checked)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(MarketplaceAdvisorShipmentUploadTask), store).CreateInstance(lifetimeScope));
            }

            if (promote.Checked && promote.Visible)
            {
                tasks.Add(new ActionTaskDescriptorBinding(typeof(MarketplaceAdvisorPromoteOrderTask), store).CreateInstance(lifetimeScope));
            }

            return tasks;
        }
    }
}
