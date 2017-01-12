using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.SingleScan.Tests
{
    public class ScannerMessageFilterFactoryTest : IDisposable
    {
        private readonly AutoMock mock;

        public ScannerMessageFilterFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void CreateFindScannerMessageFilter_DelegatesToFuncFactory()
        {
            ScannerRegistrationMessageFilter scannerRegistrationMessageFilter = mock.Create<ScannerRegistrationMessageFilter>();

            Mock<Func<ScannerRegistrationMessageFilter>> repo = mock.MockRepository.Create<Func<ScannerRegistrationMessageFilter>>();
            repo.Setup(factory => factory())
                .Returns(scannerRegistrationMessageFilter);
            mock.Provide(repo.Object);

            ScannerMessageFilterFactory testObject = mock.Create<ScannerMessageFilterFactory>();

            testObject.CreateFindScannerMessageFilter();

            repo.Verify(factory => factory());
        }

        [Fact]
        public void CreateScannerMessageFilter_DelegatesToFuncFactory()
        {
            ScannerMessageFilter scannerMessageFilter = mock.Create<ScannerMessageFilter>();

            Mock<Func<ScannerMessageFilter>> repo = mock.MockRepository.Create<Func<ScannerMessageFilter>>();
            repo.Setup(factory => factory())
                .Returns(scannerMessageFilter);
            mock.Provide(repo.Object);

            ScannerMessageFilterFactory testObject = mock.Create<ScannerMessageFilterFactory>();

            testObject.CreateMessageFilter();

            repo.Verify(factory => factory());
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}