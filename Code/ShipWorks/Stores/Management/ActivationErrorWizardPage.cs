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
        public void SetElementHost(Control control) =>
            elementHost.Child = control;
        
    }
}
