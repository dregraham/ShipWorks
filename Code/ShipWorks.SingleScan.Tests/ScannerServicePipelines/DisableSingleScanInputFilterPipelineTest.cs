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
    public class DisableSingleScanInputFilterPipelineTest : IDisposable
    {
        readonly AutoMock mock;
        private readonly Mock<ScannerService> scannerService;
        private readonly Subject<IShipWorksMessage> subject;

        public DisableSingleScanInputFilterPipelineTest()
        {
            subject = new Subject<IShipWorksMessage>();
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            scannerService = mock.Override<ScannerService>();
            mock.Provide<IObservable<IShipWorksMessage>>(subject);
        }

        [Fact]
        public void Register_DelegatesToScannerService_WhenDisableMessageIsReceived()
        {
            var testObject = mock.Create<DisableSingleScanInputFilterPipeline>();
            testObject.Register(scannerService.Object);
            subject.OnNext(new DisableSingleScanInputFilterMessage(this));

            scannerService.Verify(x => x.Disable());
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

            var testObject = mock.Create<DisableSingleScanInputFilterPipeline>();

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
