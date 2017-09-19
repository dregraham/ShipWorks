using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// UserControl that specific storetypes can derive from to create provide the automatic creation of online update actions
    /// </summary>
    public partial class OnlineUpdateActionControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OnlineUpdateActionControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gives derived controls a chance to initialize themselves with settings from the given store
        /// </summary>
        public virtual void UpdateForStore(StoreEntity store)
        {

        }

        /// <summary>
        /// Create the tasks configured to be generated.  The store they go with is given.
        /// </summary>
        public virtual List<ActionTask> CreateActionTasks(ILifetimeScope lifetimeScope, StoreEntity store)
        {
            throw new NotImplementedException();
        }
    }
}
