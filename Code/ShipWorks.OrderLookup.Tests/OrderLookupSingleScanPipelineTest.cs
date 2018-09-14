using System;
using System.Linq;
using System.Reactive.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.Settings;
using ShipWorks.Stores.Orders;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.OrderLookup.Tests
{
    public class OrderLookupSingleScanPipelineTest
    {
        private readonly AutoMock mock;
        readonly TestMessenger testMessenger;
        private readonly Mock<IOrderRepository> orderRepository;
        private readonly Mock<IMainForm> mainForm;
        private readonly OrderLookupSingleScanPipeline testObject;
        private readonly TestScheduler scheduler;

        public OrderLookupSingleScanPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();

            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.Default).Returns(scheduler);


            orderRepository = mock.Mock<IOrderRepository>();
            mainForm = mock.Mock<IMainForm>();

            mainForm.Setup(m => m.AdditionalFormsOpen()).Returns(false);
            mainForm.SetupGet(u => u.UIMode).Returns(UIMode.OrderLookup);

            testObject = mock.Create<OrderLookupSingleScanPipeline>();
            testObject.InitializeForCurrentSession();
        }
    }
}
