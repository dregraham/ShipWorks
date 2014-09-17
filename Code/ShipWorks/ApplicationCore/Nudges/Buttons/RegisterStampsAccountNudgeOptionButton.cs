using System.Windows.Forms;
using ShipWorks.Shipping.Carriers.Postal.Stamps;

namespace ShipWorks.ApplicationCore.Nudges.Buttons
{
    /// <summary>
    /// Button for a NudgeOption that will launch the Stamps.com setup wizard when clicked.
    /// </summary>
    public class RegisterStampsAccountNudgeOptionButton : AcknowledgeNudgeOptionButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterStampsAccountNudgeOptionButton"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public RegisterStampsAccountNudgeOptionButton(NudgeOption option)
            : base(option)
        { }

        /// <summary>
        /// Shows the Stamps.com setup wizard for registering a new Stamps.com account.
        /// </summary>
        public override void HandleClick()
        {
            if (HostForm != null)
            {
                using (StampsSetupWizard setupWizard = new StampsSetupWizard())
                {
                    DialogResult result = setupWizard.ShowDialog(HostForm);
                    HostForm.DialogResult = result;

                    Option.Result = result == DialogResult.OK ? "Created Stamps.com account" : "Declined to create a Stamps.com account";
                }
            }

            // Call the base acknowledge button to close the form. May want to change this in 
            // the future to only close the form if the account registration was successful.
            base.HandleClick();
        }
    }
}
