using System;
using System.Collections.Generic;
using System.Linq;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.SparkPay;

namespace ShipWorks.Stores.UI.Platforms.SparkPay
{
    /// <summary>
    /// Task for configuring the order update task for SparkPay
    /// </summary>
    public partial class SparkPayOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        readonly SparkPayOrderUpdateTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public SparkPayOrderUpdateTaskEditor(SparkPayOrderUpdateTask task)
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

            SparkPayStoreEntity store = StoreManager.GetStore(task.StoreID) as SparkPayStoreEntity;
            if (store != null)
            {
                SparkPayStatusCodeProvider statusCodeProvider = new SparkPayStatusCodeProvider(store);

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
