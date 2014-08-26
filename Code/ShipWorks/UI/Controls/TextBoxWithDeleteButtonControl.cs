using System;
using System.Windows.Forms;
using ShipWorks.UI.Utility;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// A textbox with a delete button control. Delete event is raised when delete is clicked.
    /// </summary>
    public partial class TextBoxWithDeleteButtonControl : UserControl
    {
        /// <summary>
        /// Occurs when the delete/remove button is clicked to notify interested listeners.
        /// </summary>
        public event EventHandler DeleteClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextBoxWithDeleteButtonControl"/> class.
        /// </summary>
        public TextBoxWithDeleteButtonControl(string value)
        {
            InitializeComponent();

            textBox.Text = value;

            // Get rid of the ugly bottom border on the toolstrip control
            toolStripDelete.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// Gets the value in the value box.
        /// </summary>
        public string Value
        {
            get { return textBox.Text; }
        }

        /// <summary>
        /// Called when the delete button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnDelete(object sender, EventArgs e)
        {
            if (DeleteClick != null)
            {
                DeleteClick(this, EventArgs.Empty);
            }
        }
    }
}
