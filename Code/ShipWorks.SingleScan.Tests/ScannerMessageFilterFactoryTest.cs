using System;
using Autofac.Extras.Moq;
using Moq;
using ShipWorks.Common.IO.Hardware.Scanner;
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
        public void CreateScannerRegistrationMessageFilter_ReturnsScannerRegistrationMessageFilter()
        {
            ScannerMessageFilterFactory testObject = mock.Create<ScannerMessageFilterFactory>();

            var scannerRegistrationMessageFilter = testObject.CreateScannerRegistrationMessageFilter();

            Assert.IsType<ScannerRegistrationMessageFilter>(scannerRegistrationMessageFilter);
        }

        [Fact]
        public void CreateRegisteredScannerInputHandler_ReturnsRegisteredScannerInputHandler()
        {
            ScannerMessageFilterFactory testObject = mock.Create<ScannerMessageFilterFactory>();

            var registeredScannerInputHandler = testObject.CreateRegisteredScannerInputHandler();

            Assert.IsType<RegisteredScannerInputHandler>(registeredScannerInputHandler);
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}