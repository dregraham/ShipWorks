using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Platforms.ProStores.CoreExtensions.Actions;

namespace ShipWorks.Stores.Platforms.ProStores.WizardPages
{
    /// <summary>
    /// Control for creating online update actions in the ProStores add store wizard
    /// </summary>
    public partial class ProStoresOnlineUpdateActionControl : OnlineUpdateActionControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ProStoresOnlineUpdateActionControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the UI based on the given store
        /// </summary>
        public override void UpdateForStore(StoreEntity store)
        {
            ProStoresStoreEntity proStore = (ProStoresStoreEntity) store;

            if (proStore.LoginMethod == (int) ProStoresLoginMethod.ApiToken)
            {
                shipmentUpdate.Enabled = true;
                labelOldVersion.Visible = false;
            }
            else
            {
                shipmentUpdate.Enabled = false;
                shipmentUpdate.Checked = false;
                labelOldVersion.Visible = true;
            }

            Height = labelOldVersion.Visible ? labelOldVersion.Bottom + 4 : shipmentUpdate.Bottom;
        }

        /// <summary>
        /// Create the tasks selected by the user
        /// </summary>
        public override List<ActionTask> CreateActionTasks(StoreEntity store)
        {
            if (shipmentUpdate.Checked)
            {
                return new List<ActionTask> { new ActionTaskDescriptorBinding(typeof(ProStoresShipmentUploadTask), store).CreateInstance() };
            }

            return null;
        }
    }
}
