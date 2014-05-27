using System;
using System.Windows.Forms;
using ShipWorks.UI.Utility;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ShipSenseItemAttributeControl : UserControl
    {
        /// <summary>
        /// Occurs when the delete/remove button is clicked to notify interested listeners.
        /// </summary>
        public event EventHandler DeleteAttributeClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseItemAttributeControl"/> class.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        public ShipSenseItemAttributeControl(string attributeName)
        {
            InitializeComponent();

            attribute.Text = attributeName;

            // Get rid of the ugly bottom border on the toolstrip control
            toolStripDelete.Renderer = new NoBorderToolStripRenderer();
        }

        /// <summary>
        /// Gets the name of the attribute in the text box.
        /// </summary>
        public string AttributeName
        {
            get { return attribute.Text; }
        }

        /// <summary>
        /// Called when the delete button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        public void OnDelete(object sender, EventArgs e)
        {
            if (DeleteAttributeClick != null)
            {
                DeleteAttributeClick(this, EventArgs.Empty);
            }
        }
    }
}
