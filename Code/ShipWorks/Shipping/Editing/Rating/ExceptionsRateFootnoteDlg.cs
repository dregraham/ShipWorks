using System;
using System.Windows.Forms;

namespace ShipWorks.Shipping.Editing.Rating
{
    public partial class ExceptionsRateFootnoteDlg : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionsRateFootnoteDlg"/> class.
        /// </summary>
        /// <param name="exceptionMessage">The exception message.</param>
        /// <param name="helpUrl">The help URL.</param>
        public ExceptionsRateFootnoteDlg(string exceptionMessage, string helpUrl)
        {
            InitializeComponent();

            errorMessage.Initialize(exceptionMessage, helpUrl);
        }

        /// <summary>
        /// Called when the form is loaded.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnLoad(object sender, EventArgs e)
        {
            this.Height = errorMessage.Height + ok.Height + 75;
        }
        
        /// <summary>
        /// Called when the OK button is clicked.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void OnOkClicked(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
