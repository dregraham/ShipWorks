using System;
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

        /// <summary>
        /// Create a shipping panel view model
        /// </summary>
        public static Mock<ShippingPanelViewModel> CreateShippingPanelViewModel(this AutoMock mock) =>
            CreateShippingPanelViewModel(mock, null);

        /// <summary>
        /// Create a shipping panel view model with the given configuration
        /// </summary>
        public static Mock<ShippingPanelViewModel> CreateShippingPanelViewModel(this AutoMock mock,
            Action<Mock<ShippingPanelViewModel>> configure)
        {
            var viewModel = mock.MockRepository.Create<ShippingPanelViewModel>();
            configure?.Invoke(viewModel);
            return viewModel;
        }
    }
}
