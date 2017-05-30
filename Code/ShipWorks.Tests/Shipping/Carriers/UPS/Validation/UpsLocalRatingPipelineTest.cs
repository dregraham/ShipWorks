using System;
using System.Collections.Generic;
using Autofac;
using ShipWorks.Tests.Shared;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Interapptive.Shared.UI;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.Dialogs;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.UPS.Validation
{
    public class UpsLocalRatingPipelineTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly TestMessenger messenger;
        private readonly TestScheduler windowsScheduler;
        private readonly UpsLocalRatingPipeline testObject;


        public UpsLocalRatingPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            messenger = new TestMessenger();
            mock.Provide<IMessenger>(messenger);
            windowsScheduler = new TestScheduler();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(windowsScheduler);

            testObject = mock.Create<UpsLocalRatingPipeline>();
        }

        [Fact]
        public void CallsShowMessage_WhenShipmentsProcessedMessage_AndShipmentProcessedSucessfully_AndValidatorHasMessage()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            var validationResult = mock.CreateMock<ILocalRateValidationResult>();
            validationResult.SetupGet(x => x.Message).Returns("blah");

            var processedMessage = new ShipmentsProcessedMessage(this, new[] {new ProcessShipmentResult(shipment)});
            mock.Mock<IUpsLocalRateValidator>()
                .Setup(v => v.Validate(It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Returns(validationResult.Object);

            testObject.InitializeForCurrentSession();
            messenger.Send(processedMessage);

            windowsScheduler.Start();
            validationResult.Verify(v=>v.ShowMessage(), Times.Once);
        }

        [Fact]
        public void DoesNotCallShowMessage_WhenShipmentsProcessedMessage_AndShipmentProcessedSuccessfully_AndValidatorHasNoMessage()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            var validationResult = mock.CreateMock<ILocalRateValidationResult>();

            var processedMessage = new ShipmentsProcessedMessage(this, new[] { new ProcessShipmentResult(shipment) });
            mock.Mock<IUpsLocalRateValidator>()
                .Setup(v => v.Validate(It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Returns(validationResult.Object);

            testObject.InitializeForCurrentSession();
            messenger.Send(processedMessage);

            windowsScheduler.Start();
            validationResult.Verify(v => v.ShowMessage(), Times.Never);
        }

        [Fact]
        public void DoesNotCallShowMessage_WhenShipmentProcessedMessage_AndNoShipmentProcessedSuccessfully()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            var validationResult = mock.CreateMock<ILocalRateValidationResult>();
            validationResult.SetupGet(x => x.Message).Returns("blah");

            var processedMessage = new ShipmentsProcessedMessage(this, new[] { new ProcessShipmentResult(shipment, new Exception()) });
            mock.Mock<IUpsLocalRateValidator>()
                .Setup(v => v.Validate(It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Returns(validationResult.Object);

            testObject.InitializeForCurrentSession();
            messenger.Send(processedMessage);

            windowsScheduler.Start();
            validationResult.Verify(v => v.ShowMessage(), Times.Never);
        }

        [Fact]
        public void DoesNotCallShowMessage_WhenShipmentsProcessedMessage_AndSourceIsShippingDlg()
        {
            ShipmentEntity shipment = new ShipmentEntity();
            var validationResult = mock.CreateMock<ILocalRateValidationResult>();
            validationResult.SetupGet(x => x.Message).Returns("blah");

            var shippingDlg = mock.Mock<IShippingDlg>();

            var processedMessage = new ShipmentsProcessedMessage(shippingDlg.Object, new[] { new ProcessShipmentResult(shipment) });
            mock.Mock<IUpsLocalRateValidator>()
                .Setup(v => v.Validate(It.IsAny<IEnumerable<ShipmentEntity>>()))
                .Returns(validationResult.Object);

            testObject.InitializeForCurrentSession();
            messenger.Send(processedMessage);

            windowsScheduler.Start();
            validationResult.Verify(v => v.ShowMessage(), Times.Never);
        }

        public void Dispose()
        {
            mock.Dispose();
        }

    }
}