using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Tests.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShipWorks.Shipping.Tests.ShipEngine
{
    public class ShipEngineResourceDownloaderTest
    {
        private readonly AutoMock mock;
        private readonly Mock<IHttpVariableRequestSubmitter> requestSubmitter;
        private readonly Mock<IHttpResponseReader> responseReader;
        private readonly Mock<IHttpRequestSubmitterFactory> submitterFactory;
        private readonly Mock<IApiLogEntry> logger;
        private readonly Mock<ILogEntryFactory> logFactory;

        public ShipEngineResourceDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns("result");

            requestSubmitter = mock.Mock<IHttpVariableRequestSubmitter>();
            requestSubmitter.Setup(r => r.GetResponse()).Returns(responseReader);

            submitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            submitterFactory.Setup(r => r.GetHttpVariableRequestSubmitter()).Returns(requestSubmitter);

            logger = mock.Mock<IApiLogEntry>();

            logFactory = mock.Mock<ILogEntryFactory>();
            logFactory.Setup(l => l.GetLogEntry(ApiLogSource.DHLExpress, "getlabel", LogActionType.Other)).Returns(logger);
        }

        [Fact]
        public void Download_DelegatesToHttpRequestSubmitterFactoryForRequest()
        {
            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();
            testObject.Download(new Uri("http://www.shipworks.com"), ApiLogSource.DHLExpress, "getlabel");

            submitterFactory.Verify(s => s.GetHttpVariableRequestSubmitter());
        }
        
        [Fact]
        public void Download_SetsRquestUri()
        {
            var uri = new Uri("http://www.shipworks.com");

            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();
            testObject.Download(uri, ApiLogSource.DHLExpress, "getlabel");

            requestSubmitter.VerifySet(r => r.Uri = uri);
        }

        [Fact]
        public void Download_SetsRquestVerb()
        {
            var uri = new Uri("http://www.shipworks.com");

            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();
            testObject.Download(uri, ApiLogSource.DHLExpress, "getlabel");

            requestSubmitter.VerifySet(r => r.Verb = HttpVerb.Get);
        }

        [Fact]
        public void Download_LogsRequest()
        {
            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();
            testObject.Download(new Uri("http://www.shipworks.com"), ApiLogSource.DHLExpress, "getlabel");

            logger.Verify(l => l.LogRequest(requestSubmitter.Object));
        }

        [Fact]
        public void Download_LogsResponse()
        {
            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();
            testObject.Download(new Uri("http://www.shipworks.com"), ApiLogSource.DHLExpress, "getlabel");

            logger.Verify(l => l.LogResponse("result", "txt"));
        }

        [Fact]
        public void Download_ThrowsShipEngineException_WhenHttpRequestSubmitterThrowsException()
        {
            requestSubmitter.Setup(r => r.GetResponse()).Throws(new Exception("something broke"));

            IShipEngineResourceDownloader testObject = mock.Create<ShipEngineResourceDownloader>();

            ShipEngineException ex = Assert.Throws<ShipEngineException>(() => testObject.Download(new Uri("http://www.shipworks.com"), ApiLogSource.DHLExpress, "getlabel"));

            Assert.Equal("An error occured while attempting to download reasource from DHLExpress.", ex.Message);
        }
    }
}
