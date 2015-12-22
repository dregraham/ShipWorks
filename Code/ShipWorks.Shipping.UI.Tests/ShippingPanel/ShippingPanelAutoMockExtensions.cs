using System.Diagnostics.CodeAnalysis;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.UI.Controls.AddressControl;

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
        /// Setup the ShippingPanelViewModel mock with the specified shipment
        /// </summary>
        public static Mock<ShippingPanelViewModel> WithShipment(this Mock<ShippingPanelViewModel> mock, ShipmentEntity shipment)
        {
            mock.Setup(x => x.Shipment).Returns(shipment);
            return mock;
        }

        [SuppressMessage("SonalLint", "S3215:\"interface\" instances should not be cast to concrete types",
            Justification = "We don't want to expose the property change handler to anything but tests")]
        public static void RaisePropertyChanged(this Mock<ShippingPanelViewModel> viewModel, string propertyName) =>
            RaisePropertyChanged(viewModel.Object, propertyName);

        [SuppressMessage("SonalLint", "S3215:\"interface\" instances should not be cast to concrete types",
            Justification = "We don't want to expose the property change handler to anything but tests")]
        public static void RaisePropertyChanged(this ShippingPanelViewModel viewModel, string propertyName)
        {
            ((PropertyChangedHandler) viewModel.PropertyChangeStream).RaisePropertyChanged(propertyName);
        }
    }
}
