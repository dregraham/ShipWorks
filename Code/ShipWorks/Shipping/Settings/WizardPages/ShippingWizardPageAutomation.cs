using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Shipping.Settings.WizardPages
{
    /// <summary>
    /// Wizard page for setting up automated processing actions
    /// </summary>
    public partial class ShippingWizardPageAutomation : WizardPage
    {
        ShipmentType shipmentType;
        private readonly ILifetimeScope lifetimeScope;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShippingWizardPageAutomation(ShipmentType shipmentType)
        {
            InitializeComponent();

            this.shipmentType = shipmentType;
            lifetimeScope = IoC.BeginLifetimeScope();
        }

        /// <summary>
        /// Stepping into the page
        /// </summary>
        private void OnSteppingInto(object sender, WizardSteppingIntoEventArgs e)
        {
            automationControl.EnsureInitialized(lifetimeScope, shipmentType.ShipmentTypeCode);
        }

        /// <summary>
        /// Stepping next from the page
        /// </summary>
        private void OnStepNext(object sender, WizardStepEventArgs e)
        {
            automationControl.SaveSettings();
        }
    }
}
