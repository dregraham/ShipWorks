using System.Windows.Forms;
using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Settings
{
    /// <summary>
    /// Wizard for setting up a shipment type
    /// </summary>
    public class ShipmentTypeSetupWizardForm : WizardForm
    {
        /// <summary>
        /// Run the setup wizard.  Will return false if the user canceled.
        /// </summary>
        public static DialogResult RunWizard(IWin32Window owner, ShipmentType shipmentType)
        {
            using (ILifetimeScope lifetimeScope = IoC.Current.BeginLifetimeScope())
            {
                using (ShipmentTypeSetupWizardForm wizard = shipmentType.CreateSetupWizard(lifetimeScope))
                {
                    return wizard.ShowDialog(owner);
                }
            }
        }

        /// <summary>
        /// Designed to be called from the last step of another wizard; this makes it look to the user like the setup
        /// account wizard is a seamless continuation of the previous wizard.
        /// </summary>
        /// <returns>The DialogResult of the shipment type's setup wizard.</returns>
        public static DialogResult RunFromHostWizard(WizardForm hostWizard, ShipmentType shipmentType)
        {
            // Hide the host wizard and run the setup wizard for the shipment type
            hostWizard.BeginInvoke(new MethodInvoker(hostWizard.Hide));
            return RunWizard(hostWizard, shipmentType);
        }
    }
}
