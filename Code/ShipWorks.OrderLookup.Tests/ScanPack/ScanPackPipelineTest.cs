using System;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.OrderLookup.ScanPack;
using ShipWorks.Settings;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests.ScanPack
{
    public class ScanPackPipelineTest
    {
        private readonly AutoMock mock;
        private readonly TestScheduler scheduler;
        private readonly TestMessenger testMessenger;
        private readonly Mock<ILicenseService> licenseService;
        private readonly Mock<IScanPackViewModel> scanPackViewModel;
        private readonly Mock<IMainForm> mainForm;
        private readonly ScanPackPipeline testObject;

        public ScanPackPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);

            licenseService = mock.Mock<ILicenseService>();
            scanPackViewModel = mock.Mock<IScanPackViewModel>();

            mainForm = mock.Mock<IMainForm>();
            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.Setup(m => m.IsScanPackActive()).Returns(true);
            mainForm.SetupGet(m => m.UIMode).Returns(UIMode.OrderLookup);

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            testObject = mock.Create<ScanPackPipeline>();
            testObject.InitializeForCurrentScope();
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesSingleScanMessage_WhenScanPackIsNotActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(false);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "foobar", IntPtr.Zero)));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.ProcessScan("foobar"), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesSingleScanMessage_WhenScanPackIsActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(true);

            testMessenger.Send(new SingleScanMessage(this, new ScanMessage(this, "foobar", IntPtr.Zero)));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.ProcessScan("foobar"));
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupSearchMessage_WhenScanPackIsNotActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(false);

            testMessenger.Send(new OrderLookupSearchMessage(this, "blah"));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.ProcessScan("blah"), Times.Never);
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupSearchMessage_WhenScanPackIsActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(true);

            testMessenger.Send(new OrderLookupSearchMessage(this, "blah"));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.ProcessScan("blah"));
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupLoadOrderMessage_WhenScanPackIsNotActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(false);

            var order = new OrderEntity()
            {
                OrderNumber = 123
            };

            testMessenger.Send(new OrderLookupLoadOrderMessage(this, order));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.LoadOrder(order));
        }

        [Fact]
        public void InitializeForCurrentScope_DoesNotHandlesOrderLookupLoadOrderMessage_WhenScanPackIsActive()
        {
            mainForm.Setup(m => m.IsScanPackActive()).Returns(true);

            var order = new OrderEntity()
            {
                OrderNumber = 123
            };

            testMessenger.Send(new OrderLookupLoadOrderMessage(this, order));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.LoadOrder(order), Times.Never);
        }
        
        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupClearOrderMessage_WhenClearReasonIsReset()
        {
            testMessenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.Reset));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.Reset());
        }

        [Fact]
        public void InitializeForCurrentScope_HandlesOrderLookupClearOrderMessage_WhenClearReasonIsNotReset()
        {
            testMessenger.Send(new OrderLookupClearOrderMessage(this, OrderClearReason.OrderNotFound));

            scheduler.Start();

            scanPackViewModel.Verify(s => s.Reset(), Times.Never);
        }
    }
}
