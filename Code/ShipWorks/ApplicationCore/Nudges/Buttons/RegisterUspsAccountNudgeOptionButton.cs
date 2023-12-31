﻿using System.Windows.Forms;
using Autofac;
using Interapptive.Shared.Metrics;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.ApplicationCore.Nudges.Buttons
{
    /// <summary>
    /// Button for a NudgeOption that will launch the USPS setup wizard when clicked.
    /// </summary>
    public class RegisterUspsAccountNudgeOptionButton : AcknowledgeNudgeOptionButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUspsAccountNudgeOptionButton"/> class.
        /// </summary>
        /// <param name="option">The option.</param>
        public RegisterUspsAccountNudgeOptionButton(NudgeOption option)
            : base(option)
        { }

        /// <summary>
        /// Shows the Usps setup wizard for registering a new USPS account.
        /// </summary>
        public override void HandleClick()
        {
            if (HostForm != null)
            {
                using (ILifetimeScope lifetimeScope = IoC.BeginLifetimeScope())
                {
                    IShipmentTypeSetupWizardFactory wizardFactory = lifetimeScope.Resolve<IShipmentTypeSetupWizardFactory>();
                    IShipmentTypeSetupWizard wizard = wizardFactory.Create(ShipmentTypeCode.Usps, OpenedFromSource.Nudge);

                    DialogResult result = wizard.ShowDialog(HostForm);
                    HostForm.DialogResult = result;

                    Option.Result = result == DialogResult.OK ? "Created USPS account" : "Declined to create a USPS account";
                }
            }

            // Call the base acknowledge button to close the form. May want to change this in
            // the future to only close the form if the account registration was successful.
            base.HandleClick();
        }
    }
}
