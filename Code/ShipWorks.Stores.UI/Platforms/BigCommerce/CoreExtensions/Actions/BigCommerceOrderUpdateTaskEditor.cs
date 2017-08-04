using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce;
using ShipWorks.Stores.Platforms.BigCommerce.CoreExtensions.Actions;

namespace ShipWorks.Stores.UI.Platforms.BigCommerce.CoreExtensions.Actions
{
    /// <summary>
    /// Task for configuring the order update task for BigCommerce
    /// </summary>
    public partial class BigCommerceOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        readonly BigCommerceOrderUpdateTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public BigCommerceOrderUpdateTaskEditor(BigCommerceOrderUpdateTask task)
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

            BigCommerceStoreEntity store = StoreManager.GetStore(task.StoreID) as BigCommerceStoreEntity;
            if (store != null)
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IBigCommerceStatusCodeProvider statusCodeProvider =
                        lifetimeScope.Resolve<IBigCommerceStatusCodeProvider>(TypedParameter.From(store));

                    comboBoxStatus.DisplayMember = "Key";
                    comboBoxStatus.ValueMember = "Value";

                    comboBoxStatus.DataSource = statusCodeProvider.CodeValues
                        .Select(codeValue => new KeyValuePair<string, int>(statusCodeProvider.GetCodeName(codeValue), codeValue)).ToList();
                }

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
