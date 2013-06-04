using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores.Platforms.GenericModule;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.GenericModule.CoreExtensions.Actions
{
    public partial class GenericStoreOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        GenericStoreOrderUpdateTask task = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericStoreOrderUpdateTaskEditor(GenericStoreOrderUpdateTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
            tokenBox.Text = task.Comment;

            // Listen for comment changes
            tokenBox.TextChanged += new EventHandler(OnTokenChanged);
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            comboBoxStatus.SelectedIndexChanged -= this.OnStatusChanged;
            comboBoxStatus.DataSource = null;

            GenericModuleStoreEntity store = StoreManager.GetStore(task.StoreID) as GenericModuleStoreEntity;

            if (store != null)
            {
                if (store.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.None)
                {
                    GenericModuleStoreType storeType = (GenericModuleStoreType) StoreTypeManager.GetType(store);
                    GenericStoreStatusCodeProvider statusCodeProvider = storeType.CreateStatusCodeProvider();

                    comboBoxStatus.DisplayMember = "Key";
                    comboBoxStatus.ValueMember = "Value";
                    comboBoxStatus.DataSource = statusCodeProvider.CodeValues.Select(c => new KeyValuePair<string, object>(statusCodeProvider.GetCodeName(c), c)).ToList();

                    object codeValue = !string.IsNullOrEmpty(task.StatusCode) ?
                        statusCodeProvider.ConvertCodeValue(task.StatusCode) :
                        null;

                    if (codeValue != null)
                    {
                        comboBoxStatus.SelectedValue = codeValue;
                    }

                    if (comboBoxStatus.SelectedIndex < 0 && comboBoxStatus.Items.Count > 0)
                    {
                        comboBoxStatus.SelectedIndex = 0;
                    }
                }
            }

            comboBoxStatus.SelectedIndexChanged += this.OnStatusChanged;
            OnStatusChanged(null, EventArgs.Empty);

            // Update the UI for if comments are available
            if (store == null || store.ModuleOnlineStatusSupport != (int) GenericOnlineStatusSupport.StatusWithComment)
            {
                if (panelComments.Visible)
                {
                    panelComments.Visible = false;
                    Height -= panelComments.Height;
                }
            }
            else
            {
                if (!panelComments.Visible)
                {
                    panelComments.Visible = true;
                    Height += panelComments.Height;
                }
            }
        }

        /// <summary>
        /// Change the associated task comment to the entered text 
        /// </summary>
        void OnTokenChanged(object sender, EventArgs e)
        {
            task.Comment = tokenBox.Text;
        }

        /// <summary>
        /// New status selected
        /// </summary>
        void OnStatusChanged(object sender, EventArgs e)
        {
            if (comboBoxStatus.SelectedIndex < 0)
            {
                task.StatusCode = null;
            }
            else
            {
                task.StatusCode = comboBoxStatus.SelectedValue.ToString();
            }
        }
    }
}
