using Autofac;
using ShipWorks.Core.ApplicationCode;
using ShipWorks.UI.Controls.AddressControl;
using ShipWorks.UI.Services;

namespace ShipWorks.UI
{
    /// <summary>
    /// Module registration for the ui assembly
    /// </summary>
    class UiModule : Module
    {
        /// <summary>
        /// Load the module configuration
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AddressViewModel>()
                .FindConstructorsWith(new NonDefaultConstructorFinder());

            builder.RegisterType<MessageHelperWrapper>()
                .AsImplementedInterfaces();
        }
    }
}
