using Autofac;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using ShipWorks.UI.Controls.ChannelLimit;
using ShipWorks.UI.Controls.CustomerLicenseActivation;
using ShipWorks.UI.Services;

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
                .As<ICustomerLicenseActivation>();

            builder.RegisterType<CustomerLicenseActivationViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<MessageHelperWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelLimitViewModel>();

            builder.RegisterType<ChannelLimitDlg>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelConfirmDeleteViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelConfirmDeleteDlg>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelConfirmDeleteFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicenseActivationDlg>()
                .Named<IDialog>("CustomerLicenseActivationDlg");

            builder.RegisterType<CustomerLicenseActivationDlgViewModel>()
                .AsImplementedInterfaces();

        }
    }
}