using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal;
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
        [InlineData(ShipmentTypeCode.Express1Usps, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.Express1Endicia, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.PostalWebTools, typeof(PostalShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.FedEx, typeof(WeekdaysOnlyShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.Other, typeof(OtherShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.UpsOnLineTools, typeof(WeekdaysOnlyShipmentDateManipulator))]
        [InlineData(ShipmentTypeCode.UpsWorldShip, typeof(WeekdaysOnlyShipmentDateManipulator))]
        public void EnsureShipmentDateManipulatorsAreRegisteredCorrectly(ShipmentTypeCode shipmentType, Type expectedServiceType)
        {
            IShipmentDateManipulator retriever = container.ResolveKeyed<IShipmentDateManipulator>(shipmentType);
            Assert.Equal(expectedServiceType, retriever.GetType());
        }

        [Fact]
        public void EnsureAllShipmentTypesHaveShipmentDateManipulatorRegistered()
        {
            IEnumerable<ShipmentTypeCode> excludedTypes = new[] {
                ShipmentTypeCode.Usps,
                ShipmentTypeCode.Endicia,
                ShipmentTypeCode.Express1Usps,
                ShipmentTypeCode.Express1Endicia,
                ShipmentTypeCode.PostalWebTools,
                ShipmentTypeCode.FedEx,
                ShipmentTypeCode.Other,
                ShipmentTypeCode.UpsWorldShip,
                ShipmentTypeCode.UpsOnLineTools};

            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(excludedTypes))
            {
                bool isRegistered = container.IsRegisteredWithKey<IShipmentDateManipulator>(value);
                Assert.False(isRegistered);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
