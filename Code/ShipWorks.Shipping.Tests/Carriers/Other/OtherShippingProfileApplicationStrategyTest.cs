using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Other;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Other
{
    public class OtherShippingProfileApplicationStrategyTest
    {
        private readonly ShipmentEntity shipment;
        private readonly ShippingProfileEntity profile;

        private readonly OtherShippingProfileApplicationStrategy testObject;
        private readonly Mock<ShipmentType> shipmentType;

        public OtherShippingProfileApplicationStrategyTest()
        {
            var mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            shipmentType = mock.Mock<ShipmentType>();
            var shipmentTypeManager = mock.Mock<IShipmentTypeManager>();
            shipmentTypeManager.Setup(s => s.Get(ShipmentTypeCode.Other)).Returns(shipmentType);

            shipment = new ShipmentEntity
            {
                Other = new OtherShipmentEntity
                {
                    Carrier = "shipmentCarrier", 
                    Service = "shipmentService"
                },
                Insurance = false
            };
            profile = new ShippingProfileEntity
            {
                Other = new OtherProfileEntity
                {
                    Carrier = "profileCarrier", 
                    Service = "profileService"
                },
                Insurance = true
            };

            testObject = mock.Create<OtherShippingProfileApplicationStrategy>();
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesCarrier()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileCarrier", shipment.Other.Carrier);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesService()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.Equal("profileService", shipment.Other.Service);
        }
        
        [Fact]
        public void ApplyProfile_AppliesProfilesInsurance()
        {
            testObject.ApplyProfile(profile, shipment);

            Assert.True(shipment.Other.Insurance);
        }
        
        [Fact]
        public void ApplyProfile_CallsUpdateDynamicShipmentData()
        {
            testObject.ApplyProfile(profile, shipment);

            shipmentType.Verify(s => s.UpdateDynamicShipmentData(shipment));
        }
    }
}