using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Stores.Platforms.Odbc.Upload.FieldValueResolvers;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Odbc.Upload.FieldValueResolvers
{
    public class OdbcShippingCarrierFieldValueResolverTest
    {
        [Fact]
        public void GetValue_ReturnsCorrectCarrierName()
        {
            ShipmentTypeCode testShipmentTypeCode = ShipmentTypeCode.Usps;
            string expectedCarrierName = "expected carrier name";

            using (var mock = AutoMock.GetLoose())
            {
                Mock<IShippingManager> shippingManagerMock = mock.Mock<IShippingManager>();
                shippingManagerMock.Setup(s => s.GetCarrierName(testShipmentTypeCode)).Returns(expectedCarrierName);

                var testObject = mock.Create<OdbcShippingCarrierFieldValueResolver>();
                object result = testObject.GetValue(null, new ShipmentEntity() {ShipmentTypeCode = testShipmentTypeCode});

                Assert.Equal(expectedCarrierName, result);
            }
        }

        [Fact]
        public void GetValue_ReturnsNullBeforeCallingGetCArrierName_WhenEntityNotShipmentEntity()
        {
            using (var mock = AutoMock.GetLoose())
            {
                Mock<IShippingManager> shippingManagerMock = mock.Mock<IShippingManager>();

                var testObject = mock.Create<OdbcShippingCarrierFieldValueResolver>();
                object result = testObject.GetValue(null, new OrderEntity());

                Assert.Null(result);
                shippingManagerMock.Verify(m=>m.GetCarrierName(It.IsAny<ShipmentTypeCode>()), Times.Never());
            }
        }
    }
}
