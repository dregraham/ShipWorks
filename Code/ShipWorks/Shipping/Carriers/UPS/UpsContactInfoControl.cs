using System.Windows.Forms;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Profiles;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.UPS
{
    /// <summary>
    /// A view for the UPS contact info (used in verbal confirmation) fields.
    /// </summary>
    public partial class UpsContactInfoControl : UserControl, IShippingProfileControl
    {
        /// <summary>
        /// Initializes a new <see cref="UpsContactInfoControl"/> instance.
        /// </summary>
        public UpsContactInfoControl()
        {
            InitializeComponent();
            tableLayoutPanel.Enabled = verbalConfirmationRequired.Checked;
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
            verbalConfirmationRequired.ReadMultiCheck(x => package.VerbalConfirmationEnabled = x);
            nameTextBox.ReadMultiText(x => package.VerbalConfirmationName = x);
            phoneNumberTextBox.ReadMultiText(x => package.VerbalConfirmationPhone = x);
            phoneExtensionTextBox.ReadMultiText(x => package.VerbalConfirmationPhoneExtension = x);
        }

        /// <summary>
        /// Applies package values into the controls
        /// </summary>
        /// <param name="package">Package from which values will be applied</param>
        public void ApplyFrom(UpsPackageEntity package)
        {
            verbalConfirmationRequired.ApplyMultiCheck(package.VerbalConfirmationEnabled);
            nameTextBox.ApplyMultiText(package.VerbalConfirmationName);
            phoneNumberTextBox.ApplyMultiText(package.VerbalConfirmationPhone);
            phoneExtensionTextBox.ApplyMultiText(package.VerbalConfirmationPhoneExtension);
        }

        /// <summary>
        /// Gets and sets the state in the profile
        /// </summary>
        public bool State
        {
            get { return verbalConfirmationRequired.Checked; }
            set { verbalConfirmationRequired.Checked = value; }
        }

        /// <summary>
        /// Saves the values to the specified profile
        /// </summary>
        /// <param name="entity">Profile to which the verbal confirmation information should be saved</param>
        public void SaveToEntity(EntityBase2 entity)
        {
            UpsProfilePackageEntity packageEntity = entity as UpsProfilePackageEntity;
            if (packageEntity == null)
            {
                return;
            }

            packageEntity.VerbalConfirmationEnabled = verbalConfirmationRequired.Checked;
            packageEntity.VerbalConfirmationName = nameTextBox.Text;
            packageEntity.VerbalConfirmationPhone = phoneNumberTextBox.Text;
            packageEntity.VerbalConfirmationPhoneExtension = phoneExtensionTextBox.Text;
        }

        /// <summary>
        /// Loads the values from the specified profile
        /// </summary>
        /// <param name="entity">Profile from which the verbal confirmation information should be loaded</param>
        public void LoadFromEntity(EntityBase2 entity)
        {
            UpsProfilePackageEntity packageEntity = entity as UpsProfilePackageEntity;
            if (packageEntity == null)
            {
                return;
            }

            verbalConfirmationRequired.Checked = packageEntity.VerbalConfirmationEnabled.GetValueOrDefault();
            nameTextBox.Text = packageEntity.VerbalConfirmationName;
            phoneNumberTextBox.Text = packageEntity.VerbalConfirmationPhone;
            phoneExtensionTextBox.Text = packageEntity.VerbalConfirmationPhoneExtension;
        }

        /// <summary>
        /// The verbal confirmation check box has changed
        /// </summary>
        private void OnVerbalConfirmationChanged(object sender, System.EventArgs e)
        {
            tableLayoutPanel.Enabled = verbalConfirmationRequired.Checked;
        }
    }
}
