using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Actions.Tasks;
using ShipWorks.Data.Model.EntityClasses;

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
                if (store.RestUser)
                {
                    IEnumerable<EnumEntry<Enums.ThreeDCartOrderStatus>> statuses = EnumHelper.GetEnumList<Enums.ThreeDCartOrderStatus>();
                    comboBoxStatus.DataSource = statuses.Select(s => new KeyValuePair<string, int>(s.Description, (int) s.Value)).ToList();
                }
                else
                {
                    ThreeDCartStatusCodeProvider statusCodeProvider = new ThreeDCartStatusCodeProvider(store);
                    comboBoxStatus.DataSource = statusCodeProvider.CodeValues.Select(c => new KeyValuePair<string, int>(statusCodeProvider.GetCodeName(c), c)).ToList();
                }

                comboBoxStatus.DisplayMember = "Key";
                comboBoxStatus.ValueMember = "Value";

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
                task.StatusCode = (int) comboBoxStatus.SelectedValue;
            }
        }
    }
}
