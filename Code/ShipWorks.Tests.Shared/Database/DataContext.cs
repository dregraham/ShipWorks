﻿using System;
using Autofac;
using Autofac.Extras.Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Settings;
using ShipWorks.Tests.Shared.EntityBuilders;
using ShipWorks.Users;

namespace ShipWorks.Tests.Shared.Database
{
    /// <summary>
    /// Context that is used when running a data driven test
    /// </summary>
    public class DataContext : IDisposable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DataContext(AutoMock mock, UserEntity user, ComputerEntity computer, IContainer container = null)
        {
            Mock = mock;
            User = user;
            Computer = computer;

            Store = Create.Store<GenericModuleStoreEntity>()
                .WithAddress("123 Main St.", "Suite 456", "St. Louis", "MO", "63123", "US")
                .Set(x => x.StoreName, "A Test Store")
                .Save();

            Customer = Create.Entity<CustomerEntity>().Save();

            Order = Create.Order(Store, Customer)
                .WithOrderNumber(12345)
                .WithShipAddress("1 Memorial Dr.", "Suite 2000", "St. Louis", "MO", "63102", "US")
                .Save();

            Container = container ?? (IContainer) IoC.UnsafeGlobalLifetimeScope;
        }

        /// <summary>
        /// Mock repository used for this context
        /// </summary>
        public AutoMock Mock { get; }

        /// <summary>
        /// Gets the container that's used as the IoC root container
        /// </summary>
        public IContainer Container { get; }

        /// <summary>
        /// Current user entity
        /// </summary>
        public UserEntity User { get; }

        /// <summary>
        /// Current computer entity
        /// </summary>
        public ComputerEntity Computer { get; }

        /// <summary>
        /// Default store entity
        /// </summary>
        public GenericModuleStoreEntity Store { get; private set; }

        /// <summary>
        /// Default customer entity
        /// </summary>
        public CustomerEntity Customer { get; private set; }

        /// <summary>
        /// Default order entity
        /// </summary>
        public OrderEntity Order { get; private set; }

        /// <summary>
        /// Set the default shipment type in the shipping settings
        /// </summary>
        public void SetDefaultShipmentType(ShipmentTypeCode defaultType)
        {
            UpdateShippingSetting(x => x.DefaultShipmentTypeCode = defaultType);
        }

        /// <summary>
        /// Update arbitrary shipping settings
        /// </summary>
        public void UpdateShippingSetting(Action<ShippingSettingsEntity> configureSettings)
        {
            var settings = ShippingSettings.Fetch();
            configureSettings(settings);
            ShippingSettings.Save(settings);
        }

        /// <summary>
        /// Dispose the context
        /// </summary>
        public void Dispose()
        {
            UserSession.Reset();
            Mock.Dispose();
        }
    }
}
