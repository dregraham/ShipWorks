using System.Linq;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Ups;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class UpsRateHashingServiceTest
    {
        private readonly UpsRateHashingService testObject;
        private ShipmentEntity shipment;

        public UpsRateHashingServiceTest()
        {
            testObject = new UpsRateHashingService();

            shipment = new ShipmentEntity
            {
                ShipCity = "Wildwood",
                OriginStreet1 = "123 some street",
                InsuranceProvider = 0,
                TotalWeight = 0,

                Ups = new UpsShipmentEntity()
                {
                    UpsAccountID = 1,
                    SaturdayDelivery = false,
                    CodAmount = 0,
                    CodEnabled = false,
                    CodPaymentType = 0,
                    Service = 0,
                    DeliveryConfirmation = 0
                }
            };

            shipment.Ups.Packages.Add(new UpsPackageEntity()
            {
                DimsWeight = .77,
                DimsHeight = 10,
                DimsLength = 4,
                DimsWidth = 6,
                DryIceEnabled = false,
                VerbalConfirmationEnabled = false,
                PackagingType = 0
            });
        }

        [Theory]
        [InlineData("ShipCity", "St Louis")]
        [InlineData("OriginStreet1", "456 something else street")]
        [InlineData("InsuranceProvider", 1)]
        [InlineData("TotalWeight", 5)]
        public void GetRatingHash_ReturnsDifferentHash_WhenBaseShipmentRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(ShipmentEntity).GetProperty(field);
            property.SetValue(shipment, value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }

        [Theory]
        [InlineData("UpsAccountID", 4)]
        [InlineData("SaturdayDelivery", true)]
        [InlineData("CodEnabled", true)]
        [InlineData("CodPaymentType", 1)]
        [InlineData("Service", 1)]
        [InlineData("DeliveryConfirmation", 1)]
        public void GetRatingHash_ReturnsDifferentHash_WhenUpsShipmentRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);
            
            PropertyInfo property = typeof(UpsShipmentEntity).GetProperty(field);
            property.SetValue(shipment.Ups, value, null);

            string after = testObject.GetRatingHash(shipment);
            
            Assert.NotEqual(before, after);
        }

        [Theory]
        [InlineData("DimsWeight", 4)]
        [InlineData("DimsHeight", 1)]
        [InlineData("DimsLength", 1)]
        [InlineData("DimsWidth", 1)]
        [InlineData("DryIceEnabled", true)]
        [InlineData("VerbalConfirmationEnabled", true)]
        [InlineData("PackagingType", 1)]
        public void GetRatingHash_ReturnsDifferentHash_WhenPackageRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(UpsPackageEntity).GetProperty(field);
            property.SetValue(shipment.Ups.Packages.First(), value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }
    }
}