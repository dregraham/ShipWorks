using Autofac;
using Interapptive.Shared.UI;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Communication;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.PlatforInterfaces;
using ShipWorks.Stores.Platforms.SparkPay;
using ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions;
using ShipWorks.Stores.Platforms.SparkPay.Factories;
using ShipWorks.Stores.UI.Platforms.SparkPay.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.SparkPay
{
    /// <summary>
    /// Make necessary registrations for SparkPay
    /// </summary>
    public class SparkPayModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SparkPayStoreType>()
                .Keyed<StoreType>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayDownloader>()
                .Keyed<StoreDownloader>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayInternalLicenseIdentifierFactory>()
                .Keyed<IInternalLicenseIdentifierFactory>(StoreTypeCode.SparkPay);

            builder.RegisterType<SparkPayOnlineUpdateInstanceCommandsFactory>()
                .Keyed<IOnlineUpdateInstanceCommandsFactory>(StoreTypeCode.SparkPay);

            builder.RegisterType<SparkPayStoreInstanceFactory>()
                .Keyed<IStoreInstanceFactory>(StoreTypeCode.SparkPay);

            builder.RegisterType<SparkPayOrderIdentifierFactory>()
                .Keyed<IOrderIdentifierFactory>(StoreTypeCode.SparkPay);

            builder.RegisterType<SparkPayAccountHost>()
                .Keyed<WizardPage>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayAccountViewModel>()
                .AsImplementedInterfaces();

            builder.RegisterType<SparkPayAccountControl>()
                .AsSelf();

            builder.RegisterType<SparkPayWebClient>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayStatusCodeProvider>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayShipmentFactory>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayWebClientRequestThrottle>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayOnlineUpdateActionControl>()
                .Keyed<OnlineUpdateActionControlBase>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayOnlineUpdater>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayOrderUpdateTask>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayShipmentUploadTask>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<MessageHelperWrapper>()
                .AsImplementedInterfaces();

            builder.RegisterType<SparkPayOrderUpdateTaskEditor>()
                .Keyed<ActionTaskEditor>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPaySettingsControl>()
                .Keyed<AccountSettingsControlBase>(StoreTypeCode.SparkPay)
                .ExternallyOwned();
        }
    }
}