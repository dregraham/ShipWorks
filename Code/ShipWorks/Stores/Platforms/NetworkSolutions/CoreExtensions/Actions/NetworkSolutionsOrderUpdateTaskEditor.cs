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
using ShipWorks.Stores.Platforms.NetworkSolutions;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.NetworkSolutions.CoreExtensions.Actions
{
    public partial class NetworkSolutionsOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        NetworkSolutionsOrderUpdateTask task = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkSolutionsOrderUpdateTaskEditor(NetworkSolutionsOrderUpdateTask task)
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

            NetworkSolutionsStoreEntity store = StoreManager.GetStore(task.StoreID) as NetworkSolutionsStoreEntity;

            if (store != null)
            {
                NetworkSolutionsStatusCodeProvider statusCodeProvider = new NetworkSolutionsStatusCodeProvider(store);

                comboBoxStatus.DisplayMember = "Key";
                comboBoxStatus.ValueMember = "Value";
                comboBoxStatus.DataSource = statusCodeProvider.CodeValues.Select(c => new KeyValuePair<string, object>(statusCodeProvider.GetCodeName(c), c)).ToList();

                comboBoxStatus.SelectedValue = task.StatusCode;

                if (comboBoxStatus.SelectedIndex < 0 && comboBoxStatus.Items.Count > 0)
                {
                    comboBoxStatus.SelectedIndex = 0;
                }
            }

            comboBoxStatus.SelectedIndexChanged += this.OnStatusChanged;
            OnStatusChanged(null, EventArgs.Empty);
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
                task.StatusCode = -1;
            }
            else
            {
                task.StatusCode = (long)comboBoxStatus.SelectedValue;
            }
        }
    }
}
