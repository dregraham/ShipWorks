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
using ShipWorks.Stores.Platforms.Infopia;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    public partial class InfopiaOrderUpdateTaskEditor : ActionTaskEditor
    {
        /// <summary>
        /// The task being configured
        /// </summary>
        InfopiaOrderUpdateTask task = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public InfopiaOrderUpdateTaskEditor(InfopiaOrderUpdateTask task)
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

            comboBoxStatus.Items.AddRange(InfopiaUtility.StatusValues.ToArray());
            comboBoxStatus.SelectedIndex = task.Status == null ? 0 : comboBoxStatus.Items.IndexOf(task.Status);

            comboBoxStatus.SelectedIndexChanged += this.OnStatusChanged;
        }

        /// <summary>
        /// New status selected
        /// </summary>
        void OnStatusChanged(object sender, EventArgs e)
        {
            if (comboBoxStatus.SelectedIndex < 0)
            {
                task.Status = null;
            }
            else
            {
                task.Status = comboBoxStatus.Text;
            }
        }
    }
}
