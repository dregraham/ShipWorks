using System.Windows.Forms;
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
        public static bool RunWizard(IWin32Window owner, ShipmentType shipmentType)
        {
            using (ShipmentTypeSetupWizardForm wizard = shipmentType.CreateSetupWizard())
            {
                // If it was succesful, make sure our local list of stores is refreshed
                if (wizard.ShowDialog(owner) == DialogResult.OK)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Designed to be called from the last step of another wizard; this makes it look to the user like the setup 
        /// account wizard is a seamless continuation of the previous wizard.  The DialogResult of the Best Rate Process wizard
        /// is used as the DialogResult that closes the original wizard.
        /// </summary>
        public static void ContinueAfterCreateDatabase(WizardForm originalWizard, ShipmentType shipmentType)
        {
            originalWizard.BeginInvoke(new MethodInvoker(originalWizard.Hide));

            // Run the setup wizard
            bool complete = RunWizard(originalWizard, shipmentType);

            // Counts as a cancel on the original wizard if they didn't complete the setup.
            originalWizard.DialogResult = complete ? DialogResult.OK : DialogResult.Cancel;
        }
    }
}
