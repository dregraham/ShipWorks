using Autofac;
using ShipWorks.Actions.Tasks;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.SparkPay;
using ShipWorks.Stores.Platforms.SparkPay.CoreExtensions.Actions;
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
            builder.RegisterType<SparkPayAccountHost>()
                .Keyed<WizardPage>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayAccountControl>()
                .AsSelf();

            builder.RegisterType<SparkPayStatusCodeProvider>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayOnlineUpdateActionControl>()
                .Keyed<OnlineUpdateActionControlBase>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPayOrderUpdateTask>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayShipmentUploadTask>()
                .AsSelf()
                .ExternallyOwned();

            builder.RegisterType<SparkPayOrderUpdateTaskEditor>()
                .Keyed<ActionTaskEditor>(StoreTypeCode.SparkPay)
                .ExternallyOwned();

            builder.RegisterType<SparkPaySettingsControl>()
                .Keyed<AccountSettingsControlBase>(StoreTypeCode.SparkPay)
                .ExternallyOwned();
        }
    }
}