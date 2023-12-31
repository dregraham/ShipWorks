﻿using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Threading;
using Moq;
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
            mock.Provide<ISchedulerProvider>(new ImmediateSchedulerProvider());
        }

        [Fact]
        public void Register_UpdatesOriginAddress_WhenOriginAddressTypeChanges()
        {
            var store = new StoreEntity();

            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v =>
            {
                v.Setup(x => x.OriginAddressType).Returns((long) ShipmentOriginSource.Store);
                v.Setup(x => x.OrderID).Returns(92);
                v.Setup(x => x.AccountId).Returns(102);
                v.Setup(x => x.ShipmentType).Returns(ShipmentTypeCode.Usps);
                v.Setup(x => x.ShipmentAdapter.Store).Returns(store);
            });
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity { StoreID = 23 }));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin((long) ShipmentOriginSource.Store, 92, 102, ShipmentTypeCode.Usps, store));
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0L)]
        [InlineData(-1L)]
        public void Register_DoesNotDelegateToOrigin_WhenOrderIdIsInvalid(long? testOrderId)
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v => v.Setup(x => x.OrderID).Returns(testOrderId));
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity()));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<StoreEntity>()), Times.Never());
        }

        [Theory]
        [InlineData((long) ShipmentOriginSource.Account)]
        [InlineData((long) ShipmentOriginSource.Other)]
        [InlineData(900)]
        public void Register_DoesNotCallSetAddress_WhenSourceIsNotStore(long source)
        {
            var viewModelMock = mock.CreateMock<ShippingPanelViewModel>(v => v.Setup(x => x.OriginAddressType).Returns(source));
            var viewModel = viewModelMock.Object;

            var testObject = mock.Create<StoreChangedPipeline>();
            testObject.Register(viewModel);

            testSubject.OnNext(new StoreChangedMessage(new object(), new StoreEntity()));

            viewModelMock.Verify(x => x.Origin.SetAddressFromOrigin(It.IsAny<long>(), It.IsAny<long>(),
                It.IsAny<long>(), It.IsAny<ShipmentTypeCode>(), It.IsAny<StoreEntity>()), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            testSubject.Dispose();
        }
    }
}
