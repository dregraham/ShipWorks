using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseConfirmationDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseConfirmationDlg" /> class.
        /// </summary>
        /// <param name="descriptionText">The description presented to the user regarding what is being confirmed.</param>
        public ShipSenseConfirmationDlg(string descriptionText)
        {
            InitializeComponent();

            this.descriptionText.Text = descriptionText;
            
            // Calculate the size of the text and adjust the other controls accordingly
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(this.descriptionText.Text, this.descriptionText.Font);
                this.descriptionText.Height = (int)(this.descriptionText.Height * (size.Width / this.descriptionText.Width));

                rebuildKnowledgebase.Top = this.descriptionText.Bottom + 10;
                labelContinue.Top = rebuildKnowledgebase.Bottom + 15;

                yesButton.Top = labelContinue.Bottom + 15;
                noButton.Top = yesButton.Top;

                this.Height = yesButton.Bottom + 42;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the user wants to reload the knowledge base.
        /// </summary>
        public bool IsReloadRequested
        {
            get { return rebuildKnowledgebase.Checked; }
        }
    }
}
