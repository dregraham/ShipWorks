using System;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping.UI.ShippingPanel;
using ShipWorks.Shipping.UI.ShippingPanel.ObservableRegistrations;
using ShipWorks.Stores.Content;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.UI.Tests.ShippingPanel.ObservableRegistrations
{
    public class ShipmentDeletedPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        readonly Subject<IEntityDeletedMessage> testSubject;

        public ShipmentDeletedPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            testSubject = new Subject<IEntityDeletedMessage>();
            mock.Provide<IObservable<IShipWorksMessage>>(testSubject);
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenCurrentShipmentHasDeletedId()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { ShipmentID = 123 });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new ShipmentDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_DoesNotCallUnloadShipment_WhenCurrentShipmentDoesNotHaveDeletedId()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new ShipmentDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment(), Times.Never);
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenCurrentShipmentHasDeletedOrderId()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { OrderID = 123 });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new OrderDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_DoesNotCallUnloadShipment_WhenCurrentShipmentDoesNotHaveDeletedOrderId()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>();

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new OrderDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment(), Times.Never);
        }

        [Fact]
        public void Register_DoesNotDelegateToOrderManager_WhenShipmentHasLoadedOrderAndStoreWasDeleted()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { Order = new OrderEntity { StoreID = 123 } });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            mock.Mock<IOrderManager>()
                .Verify(x => x.FetchOrder(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotDelegateToOrderManager_WhenShipmentHasLoadedOrderForADifferentStore()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { Order = new OrderEntity { StoreID = 987 } });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            mock.Mock<IOrderManager>()
                .Verify(x => x.FetchOrder(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_DelegatesToOrderManager_WhenShipmentIsForStoreButStoreIsNotLoaded()
        {
            var shipment = new ShipmentEntity();
            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(shipment);

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            mock.Mock<IOrderManager>()
                .Verify(x => x.FetchOrder(shipment.OrderID));
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenShipmentIsForStoreButStoreIsNotLoaded()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns(new OrderEntity { StoreID = 123 });

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenRelatedStoreIsNull()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns((OrderEntity) null);

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_DoesNotCallUnloadShipment_WhenCurrentShipmentDoesNotHaveDeletedStoreId()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns(new OrderEntity { StoreID = 456 });

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new StoreDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment(), Times.Never);
        }

        [Fact]
        public void Register_DoesNotDelegateToOrderManager_WhenShipmentHasLoadedOrderAndCustomerWasDeleted()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { Order = new OrderEntity { CustomerID = 123 } });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new CustomerDeletedMessage(this, 123));

            mock.Mock<IOrderManager>()
                .Verify(x => x.FetchOrder(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_DoesNotDelegateToOrderManager_WhenShipmentHasLoadedOrderForADifferentCustomer()
        {
            var viewModel = mock.CreateMock<ShippingPanelViewModel>()
                .WithShipment(new ShipmentEntity { Order = new OrderEntity { CustomerID = 987 } });

            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new CustomerDeletedMessage(this, 123));

            mock.Mock<IOrderManager>()
                .Verify(x => x.FetchOrder(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenShipmentIsForCustomerButCustomerIsNotLoaded()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns(new OrderEntity { CustomerID = 123 });

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new CustomerDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_CallsUnloadShipment_WhenRelatedCustomerIsNull()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns((OrderEntity) null);

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new CustomerDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment());
        }

        [Fact]
        public void Register_DoesNotCallUnloadShipment_WhenCurrentShipmentDoesNotHaveDeletedCustomerId()
        {
            mock.Mock<IOrderManager>()
                .Setup(x => x.FetchOrder(It.IsAny<long>()))
                .Returns(new OrderEntity { CustomerID = 456 });

            var viewModel = mock.CreateMock<ShippingPanelViewModel>().WithShipment(new ShipmentEntity());
            var testObject = mock.Create<ShipmentDeletedPipeline>();
            testObject.Register(viewModel.Object);

            testSubject.OnNext(new CustomerDeletedMessage(this, 123));

            viewModel.Verify(x => x.UnloadShipment(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
            testSubject.Dispose();
        }
    }
}
