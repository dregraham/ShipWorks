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
using ShipWorks.Stores.Platforms.AmeriCommerce;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.AmeriCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for configuring the order update task for AmeriCommerce
    /// </summary>
    public partial class AmeriCommerceOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        AmeriCommerceOrderUpdateTask task = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmeriCommerceOrderUpdateTaskEditor(AmeriCommerceOrderUpdateTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            this.task = task;
        }

        /// <summary>
        /// Initialization
        /// </summary>
        private void OnLoad(object sender, EventArgs e)
        {
            comboBoxStatus.SelectedIndexChanged -= this.OnStatusChanged;
            comboBoxStatus.DataSource = null;

            AmeriCommerceStoreEntity store = StoreManager.GetStore(task.StoreID) as AmeriCommerceStoreEntity;
            if (store != null)
            {
                AmeriCommerceStatusCodeProvider statusCodeProvider = new AmeriCommerceStatusCodeProvider(store);

                comboBoxStatus.DisplayMember = "Key";
                comboBoxStatus.ValueMember = "Value";

                comboBoxStatus.DataSource = statusCodeProvider.CodeValues.Select(c => new KeyValuePair<string, int>(statusCodeProvider.GetCodeName(c), c)).ToList();

                int code = task.StatusCode;
                if (code >= 0)
                {
                    comboBoxStatus.SelectedValue = code;
                }

                if (comboBoxStatus.SelectedIndex < 0 && comboBoxStatus.Items.Count > 0)
                {
                    comboBoxStatus.SelectedIndex = 0;
                }
            }

            comboBoxStatus.SelectedIndexChanged += this.OnStatusChanged;
        }

        /// <summary>
        /// New status selected
        /// </summary>
        void OnStatusChanged(object sender, EventArgs e)
        {
            if (comboBoxStatus.SelectedIndex < 0)
            {
                task.StatusCode = -1;
            }
            else
            {
                task.StatusCode = (int)comboBoxStatus.SelectedValue;
            }
        }
    }
}
