using System;
using System.Linq;
using Autofac.Extras.Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.Amazon
{
    public class AmazonShippingProfileApplicationStrategyTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly ShippingProfileEntity profile;
        private readonly ShipmentEntity shipment;
        private readonly PackageProfileEntity packageProfile;

        public AmazonShippingProfileApplicationStrategyTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            profile = new ShippingProfileEntity
            {
                Packages = { new PackageProfileEntity()},
                Amazon = new AmazonProfileEntity()
            };
            
            shipment = new ShipmentEntity { Amazon = new AmazonShipmentEntity() };
            packageProfile = profile.Packages.Single();
        }

        [Fact]
        public void ApplyProfile_SetsAmazonSpecificFields()
        {
            profile.Amazon.ShippingServiceID = "id";
            profile.Amazon.DeliveryExperience = 44;
            profile.Insurance = true;

            Apply();
            
            Assert.Equal("id", shipment.Amazon.ShippingServiceID);
            Assert.Equal(44, shipment.Amazon.DeliveryExperience);
            Assert.True(shipment.Amazon.Insurance);
        }
        
        [Theory]
        [InlineData(null, 42)]
        [InlineData(0, 42)]
        [InlineData(5, 5)]
        public void ApplyProfile_WeightIsSetProperly(double? profileWeight, double expectedWeight)
        {
            // Here just to make sure content weight changes or stays the same after apply 
            shipment.ContentWeight = 42;
            packageProfile.Weight = profileWeight;

            Apply();

            Assert.Equal(expectedWeight, shipment.ContentWeight);
        }
        
        [Fact]
        public void ApplyProfile_DimsProfileIDIsSet()
        {
            packageProfile.DimsProfileID = 44;

            Apply();

            Assert.Equal(44, shipment.Amazon.DimsProfileID);
        }

        [Fact]
        public void ApplyProfile_DimsensionsAreSet()
        {
            packageProfile.DimsProfileID = 44;
            packageProfile.DimsLength = 1;
            packageProfile.DimsWidth = 2;
            packageProfile.DimsHeight = 3;
            packageProfile.DimsWeight = 4;
            packageProfile.DimsAddWeight = true;

            Apply();

            Assert.Equal(44, shipment.Amazon.DimsProfileID);
            Assert.Equal(1, shipment.Amazon.DimsLength);
            Assert.Equal(2, shipment.Amazon.DimsWidth);
            Assert.Equal(3, shipment.Amazon.DimsHeight);
            Assert.Equal(4, shipment.Amazon.DimsWeight);
            Assert.True(shipment.Amazon.DimsAddWeight);
        }
        
        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeManagerWithShipment()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();

            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();

            testObject.ApplyProfile(profile, shipment);

            shipmentTypeManager.Verify(s => s.Get(shipment));
        }

        [Fact]
        public void ApplyProfile_SetsOriginIDOnShipment()
        {
            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            profile.OriginID = 123;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(123, shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_SetsReturnShipmentOnShipment()
        {
            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            profile.ReturnShipment = true;
            
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.ReturnShipment);
        }

        [Fact]
        public void ApplyProfile_SetsRequestedLabelFormatOnShipment()
        {
            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL;

            testObject.ApplyProfile(profile, shipment);

            Assert.Equal(ThermalLanguage.EPL, (ThermalLanguage) shipment.OriginOriginID);
        }

        [Fact]
        public void ApplyProfile_DelegatesToShipmentTypeToSaveLabelFormat()
        {
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            
            profile.RequestedLabelFormat = (int) ThermalLanguage.EPL ;

            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.SaveRequestedLabelFormat((ThermalLanguage) shipment.RequestedLabelFormat, shipment));
        }

        [Fact]
        public void ApplyProfile_SetsInsuranceValueOnPackage()
        {
            var insuranceChoice = mock.Mock<IInsuranceChoice>();

            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            var shipmentType = mock.Mock<ShipmentType>();
            shipmentType.Setup(s => s.GetParcelCount(shipment)).Returns(1);
            var shipmentParcel = new ShipmentParcel(shipment, null, insuranceChoice.Object, new ShipWorks.Shipping.Editing.DimensionsAdapter());

            shipmentType.Setup(s => s.GetParcelDetail(shipment, 0)).Returns(shipmentParcel);

            shipmentTypeManager.Setup(s => s.Get(shipment)).Returns(shipmentType);

            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            profile.Insurance = true;

            testObject.ApplyProfile(profile, shipment);

            insuranceChoice.VerifySet(i => i.Insured = true);
        }


        private void Apply()
        {
            var testObject = mock.Create<AmazonShippingProfileApplicationStrategy>();
            testObject.ApplyProfile(profile, shipment);
        }
        
        public void Dispose()
        {
            mock.Dispose();
        }
    }
}