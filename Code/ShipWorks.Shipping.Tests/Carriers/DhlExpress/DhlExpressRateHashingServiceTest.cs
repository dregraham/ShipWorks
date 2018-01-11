using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Dhl;
using System.Linq;
using System.Reflection;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.DhlExpress
{
    public class DhlExpressRateHashingServiceTest
    {
        private readonly DhlExpressRateHashingService testObject;
        private readonly ShipmentEntity shipment;

        public DhlExpressRateHashingServiceTest()
        {
            testObject = new DhlExpressRateHashingService();

            shipment = new ShipmentEntity
            {
                ShipCity = "Wildwood",
                OriginStreet1 = "123 some street",
                InsuranceProvider = 0,
                TotalWeight = 0,

                DhlExpress = new DhlExpressShipmentEntity()
                {
                    DhlExpressAccountID = 1,
                    SaturdayDelivery = false,
                    DeliveredDutyPaid = false,
                    NonMachinable = false,
                    Service = 0
                }
            };

            shipment.DhlExpress.Packages.Add(new DhlExpressPackageEntity()
            {
                DimsWeight = .77,
                DimsHeight = 10,
                DimsLength = 4,
                DimsWidth = 6,
                DimsAddWeight = false
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
        [InlineData("DhlExpressAccountID", 4)]
        [InlineData("SaturdayDelivery", true)]
        [InlineData("DeliveredDutyPaid", true)]
        [InlineData("NonMachinable", true)]
        [InlineData("Service", 1)]
        public void GetRatingHash_ReturnsDifferentHash_WhenDhlExpressShipmentRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(DhlExpressShipmentEntity).GetProperty(field);
            property.SetValue(shipment.DhlExpress, value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }

        [Theory]
        [InlineData("DimsWeight", 4)]
        [InlineData("DimsHeight", 1)]
        [InlineData("DimsLength", 1)]
        [InlineData("DimsWidth", 1)]
        public void GetRatingHash_ReturnsDifferentHash_WhenPackageRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(DhlExpressPackageEntity).GetProperty(field);
            property.SetValue(shipment.DhlExpress.Packages.First(), value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }
    }
}
