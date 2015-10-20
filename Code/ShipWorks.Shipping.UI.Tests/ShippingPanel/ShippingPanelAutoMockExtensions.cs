using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.AddressControl;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel
{
    public static class ShippingPanelAutoMockExtensions
    {
        public static Mock<AddressViewModel> WithOriginAddressViewModel(this AutoMock mock)
        {
            Mock<AddressViewModel> originAddress = new Mock<AddressViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .Setup(s => s.GetAddressViewModel())
                .Returns(originAddress.Object);

            return originAddress;
        }
    }
}
