using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ChangeOriginAddressFromTypePipelineTest : IDisposable
    {
        readonly AutoMock mock;

        public ChangeOriginAddressFromTypePipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void Register_UpdatesOriginAddress_WhenOriginAddressTypeChanges()
        {
            var viewModelMock = mock.CreateShippingPanelViewModel(v =>
            {
                v.Setup(x => x.OrderID).Returns(92);
                v.Setup(x => x.AccountId).Returns(102);
                v.Setup(x => x.ShipmentType).Returns(ShipmentTypeCode.Usps);
            });
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<ChangeOriginAddressFromTypePipeline>();
            testObject.Register(viewModel);

            viewModel.OriginAddressType = 23;

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(23, 92, 102, ShipmentTypeCode.Usps));
        }

        [Fact]
        public void Register_SendsZeroAsOrderId_WhenOrderIdIsNull()
        {
            var viewModelMock = mock.CreateShippingPanelViewModel(v => v.Setup(x => x.OrderID).Returns((long?) null));
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<ChangeOriginAddressFromTypePipeline>();
            testObject.Register(viewModel);

            viewModel.OriginAddressType = 23;

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), 0,
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()));
        }

        [Fact]
        public void Register_DoesNotCallSetAddress_WhenPropertyIsNotOriginAddressType()
        {
            var viewModelMock = mock.CreateShippingPanelViewModel();
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<ChangeOriginAddressFromTypePipeline>();
            testObject.Register(viewModel);

            viewModel.ShipmentType = ShipmentTypeCode.FedEx;

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
