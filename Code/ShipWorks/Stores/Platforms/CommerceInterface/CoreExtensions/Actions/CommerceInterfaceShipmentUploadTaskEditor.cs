using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Actions.Tasks.Common.Editors;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.CommerceInterface.CoreExtensions.Actions
{
    /// <summary>
    /// Task Editor for configuring the volusion shipmetn detials upload
    /// </summary>
    public partial class CommerceInterfaceShipmentUploadTaskEditor : ActionTaskEditor
    {
        // task being configured
        CommerceInterfaceShipmentUploadTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public CommerceInterfaceShipmentUploadTaskEditor(CommerceInterfaceShipmentUploadTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Load the UI
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            GenericModuleStoreEntity store = StoreManager.GetStore(task.StoreID) as GenericModuleStoreEntity;

            comboStatus.DisplayMember = "Key";
            comboStatus.ValueMember = "Value";

            comboStatus.SelectedIndexChanged -= new System.EventHandler(OnStatusChanged);
            GenericStoreStatusCodeProvider statusProvider = ((GenericModuleStoreType)StoreTypeManager.GetType(store)).CreateStatusCodeProvider();
            comboStatus.DataSource = statusProvider.CodeValues.Select(c => new KeyValuePair<string, int>(statusProvider.GetCodeName(c), Convert.ToInt32(c))).ToList();
            comboStatus.SelectedIndexChanged += new System.EventHandler(OnStatusChanged);

            comboStatus.SelectedValue = task.StatusCode;
        }


        /// <summary>
        /// A new status was selected
        /// </summary>
        private void OnStatusChanged(object sender, EventArgs e)
        {
            task.StatusCode = Convert.ToInt32(comboStatus.SelectedValue);
        }
    }
}
