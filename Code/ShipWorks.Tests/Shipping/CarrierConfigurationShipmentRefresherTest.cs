﻿using System;
using System.Collections.Generic;
using Interapptive.Shared.Messaging;
using Xunit;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Profiles;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;

namespace ShipWorks.Tests.Shipping
{
    public class CarrierConfigurationShipmentRefresherTest : IDisposable
    {
        AutoMock mock;

        MockRepository mockRepository;
        Mock<IMessenger> messengerMock;
        Mock<IShippingDialogInteraction> shippingDialogMock;
        Mock<IShippingManager> shippingManagerMock;
        Mock<IShippingProfileManager> shippingProfileManagerMock;

        ShippingProfileEntity profile;

        ShipmentEntity shipment1;
        ShipmentEntity shipment2;
        ShipmentEntity shipment3;

        private List<ShipmentEntity> shipments;
        private ShipmentEntity processingShipment;

        public CarrierConfigurationShipmentRefresherTest()
        {
            mockRepository = new MockRepository(MockBehavior.Loose) { DefaultValue = DefaultValue.Mock };
            mock = AutoMock.GetFromRepository(mockRepository);
            
            messengerMock = mockRepository.Create<IMessenger>();
            shippingDialogMock = mockRepository.Create<IShippingDialogInteraction>();
            shippingManagerMock = mockRepository.Create<IShippingManager>();
            shippingProfileManagerMock = mockRepository.Create<IShippingProfileManager>();

            shipment1 = new ShipmentEntity { ShipmentID = 1, ShipmentType = (int)ShipmentTypeCode.Usps };
            shipment2 = new ShipmentEntity { ShipmentID = 2, ShipmentType = (int)ShipmentTypeCode.Usps };
            shipment3 = new ShipmentEntity { ShipmentID = 3, ShipmentType = (int)ShipmentTypeCode.Usps };

            shipments = new List<ShipmentEntity>
            {
                shipment1, shipment2, shipment3
            };

            shippingDialogMock.Setup(x => x.FetchShipmentsFromShipmentControl()).Returns(shipments);

            profile = new ShippingProfileEntity { RequestedLabelFormat = 1 };

            shippingProfileManagerMock.Setup(x => x.GetDefaultProfile(It.IsAny<ShipmentTypeCode>())).Returns(profile);
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenMessengerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CarrierConfigurationShipmentRefresher(null, shippingDialogMock.Object, shippingProfileManagerMock.Object, shippingManagerMock.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenShipmentDialogIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CarrierConfigurationShipmentRefresher(messengerMock.Object, null, shippingProfileManagerMock.Object, shippingManagerMock.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenShippingProfileManagerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CarrierConfigurationShipmentRefresher(messengerMock.Object, shippingDialogMock.Object, null, shippingManagerMock.Object));
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException_WhenShippingManagerIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new CarrierConfigurationShipmentRefresher(messengerMock.Object, shippingDialogMock.Object, shippingProfileManagerMock.Object, null));
        }

        //[Fact]
        //public void Constructor_RegistersConfiguringCarrierMessageHandler()
        //{
        //    CarrierConfigurationShipmentRefresher refresher = CreateRefresher();
        //    messengerMock.Verify(x => x.Handle(refresher, It.IsAny<Action<ConfiguringCarrierMessage>>()));
        //}

        //[Fact]
        //public void Constructor_RegistersCarrierConfiguredMessageHandler()
        //{
        //    CarrierConfigurationShipmentRefresher refresher = CreateRefresher();
        //    messengerMock.Verify(x => x.Handle(refresher, It.IsAny<Action<CarrierConfiguredMessage>>()));
        //}

        [Fact]
        public void HandleConfiguringCarrier_DelegatesSavingAllShipments()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new ConfiguringCarrierMessage(this, ShipmentTypeCode.Usps));

            shippingDialogMock.Verify(x => x.SaveShipmentsToDatabase(shipments, true));
        }

        [Fact]
        public void HandleConfiguringCarrier_DoesNotSaveProcessedShipments()
        {
            shipment2.Processed = true;

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new ConfiguringCarrierMessage(this, ShipmentTypeCode.Usps));

            List<ShipmentEntity> expectedShipments = new List<ShipmentEntity> { shipment1, shipment3 };
            shippingDialogMock.Verify(x => x.SaveShipmentsToDatabase(expectedShipments, true));
        }

        [Fact]
        public void HandleConfiguringCarrier_DoesNotSetShipmentError_WhenNoErrorsAreReturned()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger); ;

            messenger.Send(new ConfiguringCarrierMessage(this, ShipmentTypeCode.Usps));

            shippingDialogMock.Verify(x => x.SetShipmentErrorMessage(It.IsAny<long>(), It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleConfiguringCarrier_SetsErrorDescription_WhenErrorsAreReturned()
        {
            Exception exception1 = new Exception();
            Exception exception2 = new Exception();

            shippingDialogMock.Setup(x => x.SaveShipmentsToDatabase(It.IsAny<IEnumerable<ShipmentEntity>>(), It.IsAny<bool>()))
                .Returns(new Dictionary<ShipmentEntity, Exception> {
                    { shipments[0], exception1 },
                    { shipments[2], exception2 },
                });

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new ConfiguringCarrierMessage(this, ShipmentTypeCode.Usps));

            shippingDialogMock.Verify(x => x.SetShipmentErrorMessage(1, exception1, "updated"));
            shippingDialogMock.Verify(x => x.SetShipmentErrorMessage(3, exception2, "updated"));
            shippingDialogMock.Verify(x => x.SetShipmentErrorMessage(2, It.IsAny<Exception>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void HandleConfiguringCarrier_DoesNotSaveShipments_WhenProcessing()
        {
            TestMessenger messenger = new TestMessenger();
            CarrierConfigurationShipmentRefresher refresher = CreateRefresher(messenger);

            refresher.ProcessingShipments(new List<ShipmentEntity> { new ShipmentEntity { ShipmentID = 2 } });

            messenger.Send(new ConfiguringCarrierMessage(this, ShipmentTypeCode.Usps));

            List<ShipmentEntity> expectedShipments = new List<ShipmentEntity> { shipment1, shipment3 };
            shippingDialogMock.Verify(x => x.SaveShipmentsToDatabase(expectedShipments, true));
        }

        [Fact]
        public void HandleCarrierConfigured_GetShipmentsFromControl()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            shippingDialogMock.Verify(x => x.FetchShipmentsFromShipmentControl());
        }

        [Fact]
        public void HandleCarrierConfigured_GetsDefaultProfile_ForSpecifiedCarrierType()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            shippingProfileManagerMock.Verify(x => x.GetDefaultProfile(ShipmentTypeCode.Usps));
        }

        [Fact]
        public void HandleCarrierConfigured_RefreshesShipments()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            shippingManagerMock.Verify(x => x.RefreshShipment(shipment1));
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment2));
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment3));
        }

        [Fact]
        public void HandleCarrierConfigured_SetsRequestedLabelFormat()
        {
            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(1, shipment1.RequestedLabelFormat);
            Assert.Equal(1, shipment2.RequestedLabelFormat);
            Assert.Equal(1, shipment3.RequestedLabelFormat);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotRefreshShipments_WhenProfileLabelFormatIsNull()
        {
            profile.RequestedLabelFormat = null;

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            shippingManagerMock.Verify(x => x.RefreshShipment(It.IsAny<ShipmentEntity>()), Times.Never);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotSetRequestedLabelFormat_WhenProfileLabelFormatIsNull()
        {
            profile.RequestedLabelFormat = null;

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, shipment1.RequestedLabelFormat);
            Assert.Equal(0, shipment2.RequestedLabelFormat);
            Assert.Equal(0, shipment3.RequestedLabelFormat);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotModifyShipment_WhenShipmentHasErrors()
        {
            shippingDialogMock.Setup(x => x.ShipmentHasError(shipment2.ShipmentID)).Returns(true);

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, shipment2.RequestedLabelFormat);
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment2), Times.Never);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotModifyShipment_WhenShipmentIsProcessed()
        {
            shipment2.Processed = true;

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, shipment2.RequestedLabelFormat);
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment2), Times.Never);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotModifyShipment_WhenShipmentIsDifferentType()
        {
            shipment2.ShipmentType = (int)ShipmentTypeCode.FedEx;

            TestMessenger messenger = new TestMessenger();
            CreateRefresher(messenger);

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, shipment2.RequestedLabelFormat);
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment2), Times.Never);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotModifyShipment_WhenProcessing()
        {
            TestMessenger messenger = new TestMessenger();

            CarrierConfigurationShipmentRefresher refresher = CreateRefresher(messenger);
            refresher.ProcessingShipments(new List<ShipmentEntity> { new ShipmentEntity { ShipmentID = 2 } });

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, shipment2.RequestedLabelFormat);
            shippingManagerMock.Verify(x => x.RefreshShipment(shipment2), Times.Never);
        }

        [Fact]
        public void HandleCarrierConfigured_ModifiesProcessingShipments_WhenProcessing()
        {
            TestMessenger messenger = new TestMessenger();

            CarrierConfigurationShipmentRefresher refresher = CreateRefresher(messenger);
            processingShipment = new ShipmentEntity { ShipmentID = 2, ShipmentType = (int)ShipmentTypeCode.Usps };
            refresher.ProcessingShipments(new List<ShipmentEntity> { processingShipment });

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(1, processingShipment.RequestedLabelFormat);
        }

        [Fact]
        public void HandleCarrierConfigured_DoesNotModifyShipment_WhenProcessingOtherShipmentType()
        {
            TestMessenger messenger = new TestMessenger();

            CarrierConfigurationShipmentRefresher refresher = CreateRefresher(messenger);
            processingShipment = new ShipmentEntity { ShipmentID = 2, ShipmentType = (int)ShipmentTypeCode.FedEx };
            refresher.ProcessingShipments(new List<ShipmentEntity> { processingShipment });

            messenger.Send(new CarrierConfiguredMessage(this, ShipmentTypeCode.Usps));

            Assert.Equal(0, processingShipment.RequestedLabelFormat);
        }
        
        [Fact]
        public void Dispose_UnregistersAllMessageHandlers()
        {
            var messenger = new TestMessenger();
            mock.Provide(messenger);

            //messengerMock.Setup(x => x.Handle(It.IsAny<object>(), It.IsAny<Action<CarrierConfiguredMessage>>())).Returns(token);
            var refresher = mock.Create<CarrierConfigurationShipmentRefresher>();
            refresher.Dispose();

            Assert.False(messenger.Subject.HasObservers);
            //messengerMock.Verify(x => x.Remove(token));
        }

        private CarrierConfigurationShipmentRefresher CreateRefresher()
        {
            return CreateRefresher(messengerMock.Object);
        }

        private CarrierConfigurationShipmentRefresher CreateRefresher(IMessenger messenger)
        {
            return new CarrierConfigurationShipmentRefresher(messenger, shippingDialogMock.Object, shippingProfileManagerMock.Object, shippingManagerMock.Object);
        }

        public void Dispose() => mock?.Dispose();

        private class TestMessenger : IMessenger
        {
            Subject<IShipWorksMessage> subject = new Subject<IShipWorksMessage>();

            public Subject<IShipWorksMessage> Subject => subject;

            public void Send<T>(T message) where T : IShipWorksMessage => subject.OnNext(message);

            public IDisposable Subscribe(IObserver<IShipWorksMessage> observer) => subject.Subscribe(observer);
        }
    }
}
