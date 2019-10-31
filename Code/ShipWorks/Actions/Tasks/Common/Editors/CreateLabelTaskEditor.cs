using System;
using System.Diagnostics.CodeAnalysis;

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
        [SuppressMessage("ShipWorks", "SW0002:Identifier should not be obfuscated",
        Justification = "Identifier is not being used for data binding")]
        public CreateLabelTaskEditor(CreateLabelTask task)
        {
            InitializeComponent();

            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
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
