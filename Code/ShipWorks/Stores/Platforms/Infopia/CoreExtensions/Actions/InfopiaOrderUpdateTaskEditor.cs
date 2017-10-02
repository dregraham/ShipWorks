using System;
using System.Linq;
using ShipWorks.Actions.Tasks;

namespace ShipWorks.Stores.Platforms.Infopia.CoreExtensions.Actions
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    /// THIS STORE IS DEAD
    /// This store is scheduled for removal as it no longer exists. Do not update this store when making
    /// all-platform changes.
    /// </remarks>
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
