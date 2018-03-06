using System;
using Autofac.Extras.Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Profiles
{
    public class BaseShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;

        public BaseShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity();

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { OriginID = 123 };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { ReturnShipment = true };

            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var shipment = new ShipmentEntity();
            var profile = new ShippingProfileEntity() { RequestedLabelFormat = (int)ThermalLanguage.EPL };

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var shipment = new ShipmentEntity();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            
            var profile = new ShippingProfileEntity() { RequestedLabelFormat = (int) ThermalLanguage.EPL };

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var insuranceChoice = mock.Mock<IInsuranceChoice>();

            var shipment = new ShipmentEntity();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<BaseShippingProfileApplicationStrategy>();
            var profile = new ShippingProfileEntity() { Insurance = true };

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
