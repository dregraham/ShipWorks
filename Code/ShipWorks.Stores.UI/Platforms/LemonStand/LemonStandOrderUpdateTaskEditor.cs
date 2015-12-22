using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.LemonStand;
using ShipWorks.Stores.Platforms.LemonStand.CoreExtensions.Actions;

namespace ShipWorks.Stores.UI.Platforms.LemonStand
{
    /// <summary>
    /// Task for configuring the order update task for LemonStand
    /// </summary>
    public partial class LemonStandOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        readonly LemonStandOrderUpdateTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public LemonStandOrderUpdateTaskEditor(LemonStandOrderUpdateTask task)
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

            LemonStandStoreEntity store = StoreManager.GetStore(task.StoreID) as LemonStandStoreEntity;
            if (store != null)
            {
                LemonStandStatusCodeProvider statusCodeProvider = new LemonStandStatusCodeProvider(store);

                comboBoxStatus.DisplayMember = "Key";
                comboBoxStatus.ValueMember = "Value";

                comboBoxStatus.DataSource = statusCodeProvider.CodeValues.Select(codeValue => new KeyValuePair<string, int>(statusCodeProvider.GetCodeName(codeValue), codeValue)).ToList();

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
            task.StatusCode = comboBoxStatus.SelectedIndex < 0 ? -1 : (int)comboBoxStatus.SelectedValue;
        }
    }
}
