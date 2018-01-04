using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ShipWorks.Shipping.Carriers;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Startup;
using Xunit;

namespace ShipWorks.Shipping.Tests.Integration.Carriers
{
    [Trait("Category", "ContinuousIntegration")]
    public class ShipDateManipulatorRegistrationTest : IDisposable
    {
        IContainer container;
        Dictionary<ShipmentTypeCode, Type> expectedRegistrations;

        public ShipDateManipulatorRegistrationTest()
        {
            container = ContainerInitializer.Build();
            expectedRegistrations = new Dictionary<ShipmentTypeCode, Type>
            {
                {ShipmentTypeCode.Asendia, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.DhlExpress, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.Endicia, typeof(PostalShipmentDateManipulator)},
                {ShipmentTypeCode.Express1Endicia, typeof(PostalShipmentDateManipulator)},
                {ShipmentTypeCode.Express1Usps, typeof(PostalShipmentDateManipulator)},
                {ShipmentTypeCode.FedEx, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.OnTrac, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.Other, typeof(OtherShipmentDateManipulator)},
                {ShipmentTypeCode.PostalWebTools, typeof(PostalShipmentDateManipulator)},
                {ShipmentTypeCode.UpsOnLineTools, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.UpsWorldShip, typeof(WeekdaysOnlyShipmentDateManipulator)},
                {ShipmentTypeCode.Usps, typeof(PostalShipmentDateManipulator)},
            };
        }

        [Fact]
        public void EnsureShipmentDateManipulatorsAreRegisteredCorrectly()
        {
            foreach (var expectedRegistration in expectedRegistrations)
            {
                IShipmentDateManipulator retriever = container.ResolveKeyed<IShipmentDateManipulator>(expectedRegistration.Key);
                Assert.Equal(expectedRegistration.Value, retriever.GetType());
            }            
        }

        [Fact]
        public void EnsureAllShipmentTypesAreNotRegistered()
        {
            foreach (var value in Enum.GetValues(typeof(ShipmentTypeCode)).OfType<ShipmentTypeCode>().Except(expectedRegistrations.Keys))
            {
                bool isRegistered = container.IsRegisteredWithKey<IShipmentDateManipulator>(value);
                Assert.False(isRegistered);
            }
        }

        public void Dispose() => container?.Dispose();
    }
}
