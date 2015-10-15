using Autofac.Extras.Moq;
using Interapptive.Shared.Business;
using Moq;
using ShipWorks.Shipping.Services;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.AddressControl
{
    public class AddressViewModelTest
    {
        [Fact]
        public void SetAddressFromOrigin_DelegatesToShippingOriginManager()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                var testObject = mock.Create<AddressViewModel>();
                testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

                mock.Mock<IShippingOriginManager>()
                    .Verify(x => x.GetOriginAddress(1, 2, 3, ShipmentTypeCode.Usps));
            };
        }

        [Fact]
        public void SetAddressFromOrigin_DoesNotUpdateAddress_WhenShippingOriginManagerReturnsNull()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<IShippingOriginManager>()
                    .Setup(x => x.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                    .Returns((PersonAdapter)null);

                var testObject = mock.Create<AddressViewModel>();
                testObject.City = "Foo";

                testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

                Assert.Equal("Foo", testObject.City);
            };
        }

        [Fact]
        public void SetAddressFromOrigin_UpdatesAddress_WhenShippingOriginManagerReturnsAddress()
        {
            using (AutoMock mock = AutoMockExtensions.GetLooseThatReturnsMocks())
            {
                mock.Mock<IShippingOriginManager>()
                    .Setup(x => x.GetOriginAddress(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()))
                    .Returns(new PersonAdapter { City = "Bar" });

                var testObject = mock.Create<AddressViewModel>();
                testObject.City = "Foo";

                testObject.SetAddressFromOrigin(1, 2, 3, ShipmentTypeCode.Usps);

                Assert.Equal("Bar", testObject.City);
            };
        }
    }
}
