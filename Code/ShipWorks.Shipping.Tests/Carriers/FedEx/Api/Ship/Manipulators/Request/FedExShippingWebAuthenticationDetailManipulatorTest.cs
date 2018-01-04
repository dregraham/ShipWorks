using Autofac.Extras.Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api.Ship.Manipulators.Request;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.FedEx.Api.Ship.Manipulators.Request
{
    public class FedExShippingWebAuthenticationDetailManipulatorTest
    {
        private FedExShippingWebAuthenticationDetailManipulator testObject;
        private readonly AutoMock mock;

        public FedExShippingWebAuthenticationDetailManipulatorTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testObject = mock.Create<FedExShippingWebAuthenticationDetailManipulator>();
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNull()
        {
            var result = testObject.Manipulate(new ShipmentEntity(), new ProcessShipmentRequest(), 0);

            WebAuthenticationDetail detail = result.Value.WebAuthenticationDetail;
            Assert.NotNull(detail);
        }

        [Fact]
        public void Manipulate_SetsWebAuthenticationDetail_WhenWebAuthenticationDetailsIsNotNull()
        {
            var request = new ProcessShipmentRequest { WebAuthenticationDetail = new WebAuthenticationDetail() };

            var result = testObject.Manipulate(new ShipmentEntity(), request, 0);

            WebAuthenticationDetail detail = result.Value.WebAuthenticationDetail;
            Assert.NotNull(detail);
        }
    }
}
