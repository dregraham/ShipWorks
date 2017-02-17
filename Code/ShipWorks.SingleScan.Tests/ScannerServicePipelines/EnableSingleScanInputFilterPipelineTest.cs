using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Autofac.Extras.Moq;
using Interapptive.Shared.Messaging;
using Moq;
using ShipWorks.Messaging.Messages.SingleScan;
using ShipWorks.SingleScan.ScannerServicePipelines;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests.ScannerServicePipelines
{
    public class EnableSingleScanInputFilterPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ScannerService> scannerService;
        private readonly Subject<IShipWorksMessage> subject;

        public EnableSingleScanInputFilterPipelineTest()
        {
            subject = new Subject<IShipWorksMessage>();
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            scannerService = mock.Override<ScannerService>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Register_DoesNotDelegateToScannerService_WhenSingleScanIsDisabled()
        {
            scannerService.Setup(x => x.IsSingleScanEnabled()).Returns(false);

            var testObject = mock.Create<EnableSingleScanInputFilterPipeline>();
            testObject.Register(scannerService.Object);
            subject.OnNext(new EnableSingleScanInputFilterMessage(this));

            scannerService.Verify(x => x.Enable(), Times.Never);
        }

        [Fact]
        public void Register_DelegatesToScannerService_WhenSingleScanIsEnabled()
        {
            scannerService.Setup(x => x.IsSingleScanEnabled()).Returns(true);

            var testObject = mock.Create<EnableSingleScanInputFilterPipeline>();
            testObject.Register(scannerService.Object);
            subject.OnNext(new EnableSingleScanInputFilterMessage(this));

            scannerService.Verify(x => x.Enable());
        }

        [Fact]
        public void Register_ReturnsDisposable_FromObservable()
        {
            var disposable = mock.CreateMock<IDisposable>();
            var stream = Observable.Create<IShipWorksMessage>(x =>
            {
                return disposable.Object;
            });
            mock.Provide(stream);

            var testObject = mock.Create<EnableSingleScanInputFilterPipeline>();
            scannerService.Setup(x => x.IsSingleScanEnabled()).Returns(true);

            var result = testObject.Register(scannerService.Object);
            result.Dispose();

            disposable.Verify(x => x.Dispose());
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
