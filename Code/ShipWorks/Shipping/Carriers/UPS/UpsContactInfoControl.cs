using System.Windows.Forms;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// A view for the UPS contact info (used in verbal confirmation) fields.
    /// </summary>
    public partial class UpsContactInfoControl : UserControl
    {
        /// <summary>
        /// Initializes a new <see cref="UpsContactInfoControl"/> instance.
        /// </summary>
        public UpsContactInfoControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the contact name.
        /// </summary>
        public string ContactName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string PhoneNumber
        {
            get { return phoneNumberTextBox.Text; }
            set { phoneNumberTextBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets the phone extension.
        /// </summary>
        public string PhoneExtension
        {
            get { return phoneExtensionTextBox.Text; }
            set { phoneExtensionTextBox.Text = value; }
        }
    }
}
