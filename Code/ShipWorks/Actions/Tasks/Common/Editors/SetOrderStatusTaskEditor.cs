using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.Actions.Tasks.Common.Editors
{
    /// <summary>
    /// Editor for the Set Order Status task
    /// </summary>
    public partial class SetOrderStatusTaskEditor : ActionTaskEditor
    {
        SetOrderStatusTask task;

        /// <summary>
        /// Constructor
        /// </summary>
        public SetOrderStatusTaskEditor(SetOrderStatusTask task)
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
            tokenBox.Text = task.Status;
            tokenBox.TextChanged += new EventHandler(OnTextChanged);
        }

        /// <summary>
        /// The token status text has changed
        /// </summary>
        void OnTextChanged(object sender, EventArgs e)
        {
            task.Status = tokenBox.Text;
        }
    }
}
