using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ShipmentControl;
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

        public static Mock<AddressViewModel> WithDestinationAddressViewModel(this AutoMock mock)
        {
            Mock<AddressViewModel> destinationAddress = new Mock<AddressViewModel>();

            mock.Mock<IShippingViewModelFactory>()
                .Setup(s => s.GetAddressViewModel())
                .Returns(destinationAddress.Object);

            return destinationAddress;
        }

        /// <summary>
        /// Setup the ShippingPanelViewModel mock with the specified shipment
        /// </summary>
        public static Mock<ShippingPanelViewModel> WithShipment(this Mock<ShippingPanelViewModel> mock, ShipmentEntity shipment)
        {
            mock.Setup(x => x.Shipment).Returns(shipment);
            return mock;
        }

        /// <summary>
        /// Setup the ShippingPanelViewModel mock with the specified shipment
        /// </summary>
        public static Mock<ShippingPanelViewModel> WithShipmentViewModel(this Mock<ShippingPanelViewModel> mock)
        {
            Mock<ShipmentViewModel> shipmentViewModel = new Mock<ShipmentViewModel>();
            Mock<InsuranceViewModel> insuranceViewModel = new Mock<InsuranceViewModel>();

            shipmentViewModel.Setup(svm => svm.InsuranceViewModel).Returns(insuranceViewModel.Object);

            mock.Setup(x => x.ShipmentViewModel).Returns(shipmentViewModel.Object);
            shipmentViewModel.Setup(x => x.InsuranceViewModel).Returns(insuranceViewModel.Object);

            return mock;
        }
    }
}
