using Autofac;
using ShipWorks.UI.Controls;
using ShipWorks.UI.Wizard;

namespace ShipWorks.UI
{
    public class UIModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerLicenseActivationControlHost>()
                .Named<WizardPage>("CustomerLicenseActivationControlHost")
                .ExternallyOwned();

            builder.RegisterType<CustomerLicenseActivationViewModel>();
        }
    }
}