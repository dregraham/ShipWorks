using System;
using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Editions;
using ShipWorks.UI.Controls.ChannelConfirmDelete;
using ShipWorks.UI.Controls.ChannelLimit;
using ShipWorks.UI.Controls.ChannelLimit.ChannelLimitBehavior;
using ShipWorks.UI.Controls.CustomerLicenseActivation;
using ShipWorks.UI.Controls.WebBrowser;

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

            builder.RegisterType<ChannelLimitViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelLimitFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelLimitControl>()
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<ChannelLimitDlg>()
                .Named<IDialog>("ChannelLimitDlg");

            builder.RegisterType<ChannelConfirmDeleteViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<ChannelConfirmDeleteDlg>()
                .Named<IDialog>("ChannelConfirmDeleteDlg");

            builder.RegisterType<ChannelConfirmDeleteFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<CustomerLicenseActivationDlg>()
                .Named<IDialog>("CustomerLicenseActivationDlg");

            builder.RegisterType<CustomerLicenseActivationDlgViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<WebBrowserDlgViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<WebBrowserDlg>()
                .Named<IDialog>("WebBrowserDlg");

            builder.RegisterType<WebBrowserFactory>()
                .AsImplementedInterfaces();

            builder.Register<Func<string, IDialog>>(
                componentContext =>
                {
                    IComponentContext resolved = componentContext.Resolve<IComponentContext>();
                    return named => resolved.ResolveNamed<IDialog>(named);
                });

            builder.RegisterType<ChannelLimitDlgFactory>()
                .AsImplementedInterfaces();

            builder.RegisterType<OverChannelLimitBehavior>()
                .Keyed<IChannelLimitBehavior>(EditionFeature.ChannelCount);

            builder.RegisterType<GenericFileBehavior>()
                .Keyed<IChannelLimitBehavior>(EditionFeature.GenericFile);

            builder.RegisterType<GenericModuleBehavior>()
                .Keyed<IChannelLimitBehavior>(EditionFeature.GenericModule);
        }
    }
}