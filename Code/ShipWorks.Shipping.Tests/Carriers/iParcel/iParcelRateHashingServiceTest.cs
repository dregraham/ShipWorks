using System.Linq;
using System.Reflection;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Ups
{
    public class iParcelRateHashingServiceTest
    {
        private readonly iParcelRateHashingService testObject;
        private ShipmentEntity shipment;

        public iParcelRateHashingServiceTest()
        {
            testObject = new iParcelRateHashingService();

            shipment = new ShipmentEntity
            {
                ShipCity = "Wildwood",
                OriginStreet1 = "123 some street",
                InsuranceProvider = 0,
                TotalWeight = 0,

                IParcel = new IParcelShipmentEntity()
                {
                    IParcelAccountID = 1,
                    IsDeliveryDutyPaid = false,
                    TrackBySMS = false
                }
            };

            shipment.IParcel.Packages.Add(new IParcelPackageEntity()
            {
                Weight = 1,
                DimsHeight = 10,
                DimsLength = 4,
                DimsWidth = 6,
                InsurancePennyOne = false
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
        [InlineData("IParcelAccountID", 4)]
        [InlineData("IsDeliveryDutyPaid", true)]
        [InlineData("TrackBySMS", true)]
        public void GetRatingHash_ReturnsDifferentHash_WhenUpsShipmentRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(IParcelShipmentEntity).GetProperty(field);
            property.SetValue(shipment.IParcel, value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }

        [Theory]
        [InlineData("Weight", 2)]
        [InlineData("DimsHeight", 1)]
        [InlineData("DimsLength", 2)]
        [InlineData("DimsWidth", 3)]
        [InlineData("InsurancePennyOne", true)]
        public void GetRatingHash_ReturnsDifferentHash_WhenPackageRatingFieldChanges(string field, object value)
        {
            string before = testObject.GetRatingHash(shipment);

            PropertyInfo property = typeof(IParcelPackageEntity).GetProperty(field);
            property.SetValue(shipment.IParcel.Packages.First(), value, null);

            string after = testObject.GetRatingHash(shipment);

            Assert.NotEqual(before, after);
        }
    }
}