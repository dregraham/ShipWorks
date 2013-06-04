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
using ShipWorks.Stores.Platforms.ThreeDCart;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.ThreeDCart.CoreExtensions.Actions
{
    /// <summary>
    /// Task for configuring the order update task for ThreeDCart
    /// </summary>
    public partial class ThreeDCartOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        readonly ThreeDCartOrderUpdateTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public ThreeDCartOrderUpdateTaskEditor(ThreeDCartOrderUpdateTask task)
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
            comboBoxStatus.SelectedIndexChanged -= OnStatusChanged;
            comboBoxStatus.DataSource = null;

            ThreeDCartStoreEntity store = StoreManager.GetStore(task.StoreID) as ThreeDCartStoreEntity;
            if (store != null)
            {
                ThreeDCartStatusCodeProvider statusCodeProvider = new ThreeDCartStatusCodeProvider(store);

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

            comboBoxStatus.SelectedIndexChanged += OnStatusChanged;
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
