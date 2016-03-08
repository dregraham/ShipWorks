using System.Drawing;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace ShipWorks.Stores.Management
{
    /// <summary>
    /// A page for use in the AddStoreWizard - Show license activation errors
    /// </summary>
    public partial class ActivationErrorWizardPage : AddStoreWizardPage
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ActivationErrorWizardPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// ErrorMessage to display.
        /// </summary>
        public string ErrorMessage
        {
            get { return errorMessage.Text; }
            set { errorMessage.Text = value; }
        }

        /// <summary>
        /// Gets the element host.
        /// </summary>
        public void SetElementHost(Control control)
        {
            errorMessage.Visible = false;
            elementHost.Location = errorMessage.Location;

            elementHost.Child = control;

            // There was a weird issue where the background of the WPF control was black.
            // This fixes it.
            Color color = SystemColors.Control;
            control.Background = new System.Windows.Media.SolidColorBrush(
              System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

            elementHost.Visible = true;
        }
    }
}
