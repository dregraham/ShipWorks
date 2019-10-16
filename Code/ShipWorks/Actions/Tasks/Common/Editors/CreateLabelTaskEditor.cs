using System;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Editor for the Create Label task
    /// </summary>
    public partial class CreateLabelTaskEditor : ActionTaskEditor
    {
        CreateLabelTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public CreateLabelTaskEditor(CreateLabelTask task)
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
            allowMultiShipments.Checked = task.AllowMultiShipments;
            allowMultiShipments.CheckedChanged += new EventHandler(MultiShipmentsChanged);
        }

        /// <summary>
        /// The allowMultiShipments checkbox was changed
        /// </summary>
        void MultiShipmentsChanged(object sender, EventArgs e)
        {
            task.AllowMultiShipments = allowMultiShipments.Checked;
        }
    }
}
