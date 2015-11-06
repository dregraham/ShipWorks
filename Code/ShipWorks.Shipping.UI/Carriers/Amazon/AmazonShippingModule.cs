﻿using Autofac;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Shipping.Settings;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// Service registrations for the Amazon shipping carrier
    /// </summary>
    public class AmazonShippingModule : Module
    {
        /// <summary>
        /// Load the registrations
        /// </summary>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<AmazonShippingWebClient>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<AmazonCredentials>()
                .AsSelf()
                .As<IAmazonCredentials>();

            builder.RegisterType<AmazonShipmentSetupWizard>()
                .Keyed<ShipmentTypeSetupWizardForm>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonAccountEditorDlg>();
            builder.RegisterType<AmazonAccountEditorViewModel>();

            builder.RegisterType<AmazonSettingsControl>()
                .Keyed<SettingsControlBase>(ShipmentTypeCode.Amazon)
                .InstancePerLifetimeScope();

            builder.RegisterType<AmazonShipmentType>()
                .AsSelf()
                .Keyed<ShipmentType>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonAccountManager>()
                .AsImplementedInterfaces()
                .As<AccountManagerBase<AmazonAccountEntity>>()
                .SingleInstance();

            builder.RegisterType<AmazonServiceControl>()
                .UsingConstructor(typeof(RateControl), typeof(AmazonServiceViewModel), typeof(AmazonShipmentType))
                .Keyed<ServiceControlBase>(ShipmentTypeCode.Amazon)
                .ExternallyOwned();

            builder.RegisterType<AmazonServiceViewModel>();

            builder.RegisterType<AmazonShipmentProcessingSynchronizer>()
                .Keyed<IShipmentProcessingSynchronizer>(ShipmentTypeCode.Amazon)
                .SingleInstance();

            builder.RegisterType<AmazonRatingService>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonMwsWebClientSettingsFactory>()
                .As<IAmazonMwsWebClientSettingsFactory>();

            builder.RegisterType<AmazonProfileControl>()
                .Keyed<ShippingProfileControlBase>(ShipmentTypeCode.Amazon);

            builder.RegisterType<AmazonLabelService>()
                .AsImplementedInterfaces();

            builder.RegisterType<AmazonShipmentRequestDetailsFactory>()
                .As<IAmazonShipmentRequestDetailsFactory>();

            builder.RegisterType<AmazonNotLinkedFootnoteFactory>()
                .As<IAmazonNotLinkedFootnoteFactory>()
                .ExternallyOwned();

            if (!(InterapptiveOnly.IsInterapptiveUser ^ InterapptiveOnly.MagicKeysDown))
            {
                builder.RegisterType<AmazonUspsRateFilter>()
                    .AsImplementedInterfaces();

                builder.RegisterType<AmazonUspsLabelEnforcer>()
                    .AsImplementedInterfaces();

                builder.RegisterType<AmazonUpsRateFilter>()
                    .AsImplementedInterfaces();

                builder.RegisterType<AmazonUpsLabelEnforcer>()
                    .AsImplementedInterfaces();
            }
        }
    }
}
