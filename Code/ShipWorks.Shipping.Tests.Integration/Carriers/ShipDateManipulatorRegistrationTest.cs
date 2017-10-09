using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.Postal.Express1;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Startup;
using Xunit;
using ShipWorks.Shipping.Carriers;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class ShipDateManipulatorRegistrationTest : IDisposable
    {
        IContainer container;

        public ShipDateManipulatorRegistrationTest()
        {
            container = new ContainerBuilder().Build();
            ContainerInitializer.BuildRegistrations(container);
        }

        [Theory]
        [InlineData(ShipmentTypeCode.Usps, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.Endicia, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.Other, typeof(OtherShipmentDateManipulator))]
        public void EnsureShipmentDateManipulatorsAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            IShipmentDateManipulator retriever = container.ResolveKeyed<IShipmentDateManipulator>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllShipmentTypesHaveShipmentDateManipulatorRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] { ShipmentTypeCode.Usps, ShipmentTypeCode.Endicia, ShipmentTypeCode.PostalWebTools, ShipmentTypeCode.Other };

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                bool isRegistered = container.IsRegisteredWithKey<IShipmentDateManipulator>(value);
                Assert.False(isRegistered);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
