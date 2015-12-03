using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class StoreChangedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IShipWorksMessage> testSubject;

        public StoreChangedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testSubject = new Subject<IShipWorksMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(testSubject);
        }

        [Fact]
        public void Register_UpdatesOriginAddress_WhenOriginAddressTypeChanges()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.OriginAddressType).Returns((long) ShipmentOriginSource.Store);
                v.Setup(x => x.OrderID).Returns(92);
                v.Setup(x => x.AccountId).Returns(102);
                v.Setup(x => x.ShipmentType).Returns(ShipmentTypeCode.Usps);
            });
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity { StoreID = 23 }));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin((long) ShipmentOriginSource.Store, 92, 102, ShipmentTypeCode.Usps));
        }

        [Fact]
        public void Register_SendsZeroAsOrderId_WhenOrderIdIsNull()
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v => v.Setup(x => x.OrderID).Returns((long?) null));
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity()));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), 0,
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()));
        }

        [Theory]
        [InlineData(ShipmentOriginSource.Account)]
        [InlineData(ShipmentOriginSource.Other)]
        [InlineData(900)]
        public void Register_DoesNotCallSetAddress_WhenSourceIsNotStore(long source)
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v => v.Setup(x => x.OriginAddressType).Returns(source));
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity()));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            testSubject.Dispose();
        }
    }
}
