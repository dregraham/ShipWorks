using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.UI.Controls;

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

        /// <summary>
        /// Reads values from the control into the package
        /// </summary>
        /// <param name="package">Package into which values will be read</param>
        public void ReadInto(UpsPackageEntity package)
        {
            package.VerbalConfirmationName = ContactName;
            package.VerbalConfirmationPhone = PhoneNumber;
            package.VerbalConfirmationPhoneExtension = PhoneExtension;
        }

        /// <summary>
        /// Applies package values into the controls
        /// </summary>
        /// <param name="package">Package from which values will be applied</param>
        public void ApplyFrom(UpsPackageEntity package)
        {
            ContactName = package.VerbalConfirmationName;
            PhoneNumber = package.VerbalConfirmationPhone;
            PhoneExtension = package.VerbalConfirmationPhoneExtension;
        }
    }
}
