﻿using Autofac;
using Interapptive.Shared.Metrics;
using RestSharp;
using ShipWorks.Stores.Orders;
using ShipWorks.Stores.Services;

namespace ShipWorks.Stores.UI
{
    /// <summary>
    /// Module for stores assembly
    /// </summary>
    public class StoreModule : Module
    {
        /// <summary>
        /// Loads the specified builder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<StoreManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<StoreTypeManagerWrapper>()
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterType<OrderRepository>()
                .As<IOrderRepository>()
                .SingleInstance();

            builder.RegisterType<OrderNote>()
                .As<IOrderNote>();

            builder.RegisterType<OrderChargeCalculator>()
                .As<IOrderChargeCalculator>();

            builder.RegisterType<RestClient>()
                .As<IRestClient>();

            builder.RegisterType<StoreSettingsTrackedDurationEvent>()
                .As<IStoreSettingsTrackedDurationEvent>();
        }
    }
}
