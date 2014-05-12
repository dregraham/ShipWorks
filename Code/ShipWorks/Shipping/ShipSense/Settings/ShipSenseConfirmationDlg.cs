using System.Drawing;
using System.Windows.Forms;

namespace ShipWorks.Shipping.ShipSense.Settings
{
    public partial class ShipSenseConfirmationDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipSenseConfirmationDlg" /> class.
        /// </summary>
        /// <param name="confirmationText">The confirmation text.</param>
        public ShipSenseConfirmationDlg(string confirmationText)
        {
            InitializeComponent();

            this.confirmationText.Text = confirmationText;
            
            // Calculate the size of the text and adjust the other controls accordingly
            using (Graphics g = CreateGraphics())
            {
                SizeF size = g.MeasureString(this.confirmationText.Text, this.confirmationText.Font);
                this.confirmationText.Height = (int)(this.confirmationText.Height * (size.Width / this.confirmationText.Width));

                rebuildKnowledgebase.Top = this.confirmationText.Bottom + 10;
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
