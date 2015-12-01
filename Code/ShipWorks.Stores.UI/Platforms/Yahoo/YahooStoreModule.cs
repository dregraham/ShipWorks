using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Core;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Management;
using ShipWorks.Stores.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.Platforms.Yahoo.EmailIntegration;
using ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration;
using ShipWorks.Stores.UI.Platforms.Yahoo.ApiIntegration.WizardPages;
using ShipWorks.UI.Wizard;

namespace ShipWorks.Stores.UI.Platforms.Yahoo
{
    /// <summary>
    /// Register YahooStore
    /// </summary>
    public class YahooStoreModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<YahooApiAccountPageViewModel>();
            builder.RegisterType<YahooApiAccountSettingsViewModel>();

            builder.RegisterType<YahooApiAccountPageHost>()
                .Keyed<WizardPage>(StoreTypeCode.Yahoo)
                .ExternallyOwned();

            builder.RegisterType<YahooApiWebClient>().AsImplementedInterfaces();

            builder.Register(GetAccountSettingsControlBase)
                .Keyed<AccountSettingsControlBase>(StoreTypeCode.Yahoo)
                .ExternallyOwned();
        }

        /// <summary>
        /// Gets the account settings control base.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">Store is not a Yahoo Store</exception>
        private static AccountSettingsControlBase GetAccountSettingsControlBase(IComponentContext context, IEnumerable<Parameter> parameters)
        {
            YahooStoreEntity store = parameters.TypedAs<StoreEntity>() as YahooStoreEntity;

            if (store == null)
            {
                throw new InvalidOperationException("Store is not a Yahoo Store");
            }

            return string.IsNullOrWhiteSpace(store.YahooStoreID) ?
                new YahooEmailAccountSettingsControl() : 
                (AccountSettingsControlBase) new YahooApiAccountSettingsHost();
        }
    }
}