using Autofac.Extras.Moq;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Api;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonShippingWebClientTest
    {
        [Fact]
        public void ValidateCreateShipmentResponse_ThrowsAmazonShipperExceptionWhen_ShipmentResponseIsNull()
        {
            using (var mock = AutoMock.GetLoose())
            {
                IAmazonShippingWebClient client = mock.Create<IAmazonShippingWebClient>();
                
                Assert.Throws<AmazonShipperException>(() => client.ValidateCreateShipmentResponse(null));
            }
        }
    }
}
