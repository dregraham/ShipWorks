using Autofac.Extras.Moq;
using Interapptive.Shared.Threading;
using Microsoft.Reactive.Testing;
using Moq;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Core.Messaging;
using ShipWorks.Editions;
using ShipWorks.OrderLookup.ScanPack;
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
        private readonly ScanPackPipeline testObject;

        public ScanPackPipelineTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            Mock<ISchedulerProvider> scheduleProvider = mock.WithMockImmediateScheduler();
            scheduler = new TestScheduler();
            scheduleProvider.Setup(s => s.WindowsFormsEventLoop).Returns(scheduler);

            licenseService = mock.Mock<ILicenseService>();
            scanPackViewModel = mock.Mock<IScanPackViewModel>();

            testMessenger = new TestMessenger();
            mock.Provide<IMessenger>(testMessenger);

            testObject = mock.Create<ScanPackPipeline>();
        }

        [Fact]
        public void InitializeForCurrentScope_DisablesScanPack_WhenFeatureRestricted()
        {
            licenseService
                .Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.Forbidden);

            testObject.InitializeForCurrentScope();

            scanPackViewModel.Verify(s => s.Enabled == false);
        }

        [Fact]
        public void InitializeForCurrentScope_EnablesScanPack_WhenFeatureRestricted()
        {
            licenseService
                .Setup(l => l.CheckRestriction(EditionFeature.Warehouse, null))
                .Returns(EditionRestrictionLevel.None);

            testObject.InitializeForCurrentScope();

            scanPackViewModel.Verify(s => s.Enabled == true);
        }
    }
}
