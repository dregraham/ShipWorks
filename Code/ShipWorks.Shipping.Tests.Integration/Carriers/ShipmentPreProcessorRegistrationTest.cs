﻿using System;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    [Trait("Category", "IoCRegistration")]
    public class ShipmentPreProcessorRegistrationTest : IDisposable
    {
        IContainer container;

        public ShipmentPreProcessorRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.Initialize(container);
        }

        [Fact]
        public void EnsureBestRateUsesBestRateShipmentPreProcessor()
        {
            IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(ShipmentTypeCode.BestRate);
            Assert.IsType<BestRateShipmentPreProcessor>(service);
        }

        [Fact]
        public void EnsureUspsUsesUspsShipmentPreProcessor()
        {
            IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(ShipmentTypeCode.Usps);
            Assert.IsType<UspsShipmentPreProcessor>(service);
        }

        [Fact]
        public void EnsureAllCarriersExceptBestRateUseGenericShipmentPreProcessor()
        {
            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Where(x => x != ShipmentTypeCode.BestRate && x != ShipmentTypeCode.Usps))
            {
                IShipmentPreProcessor service = container.ResolveKeyed<IShipmentPreProcessor>(value);
                Assert.IsType<DefaultShipmentPreProcessor>(service);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
