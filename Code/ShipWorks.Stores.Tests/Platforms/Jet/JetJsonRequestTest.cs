using System;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.Net;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JetJsonRequestTest: IDisposable
    {
        private readonly AutoMock mock;

        public JetJsonRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ProcessRequest_LogsRequest()
        {
            var response = mock.Mock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]");

            var submitter = mock.Mock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            Mock<IApiLogEntry> logEntry = mock.Mock<IApiLogEntry>();
            mock.Mock<ILogEntryFactory>()
                .Setup(l => l.GetLogEntry(ApiLogSource.Jet, "ProcessRequest", LogActionType.Other))
                .Returns(logEntry);

            var testObject = mock.Create<JsonRequest>();
            testObject.Submit<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            logEntry.Verify(e => e.LogRequest(submitter.Object), Times.Once());
        }

        [Fact]
        public void ProcessRequestLogsResponse_WhenWebRequestSucessful()
        {
            var response = mock.Mock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]");

            var submitter = mock.Mock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            Mock<IApiLogEntry> logEntry = mock.Mock<IApiLogEntry>();
            mock.Mock<ILogEntryFactory>()
                .Setup(l => l.GetLogEntry(ApiLogSource.Jet, "ProcessRequest", LogActionType.Other))
                .Returns(logEntry);

            var testObject = mock.Create<JsonRequest>();
            testObject.Submit<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            logEntry.Verify(e => e.LogResponse("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]", "json"), Times.Once());
        }

        [Fact]
        public void ProcessRequest_LogsResponse_WhenWebRequestFails()
        {
            WebException exception = new WebException();

            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Throws(exception);

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            Mock<IApiLogEntry> logEntry = mock.Mock<IApiLogEntry>();
            mock.Mock<ILogEntryFactory>()
                .Setup(l => l.GetLogEntry(ApiLogSource.Jet, "ProcessRequest", LogActionType.Other))
                .Returns(logEntry);

            var testObject = mock.Create<JsonRequest>();

            Assert.Throws<WebException>( () =>
                testObject.Submit<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object));

            logEntry.Verify(e => e.LogResponse(exception), Times.Once());
        }


        [Fact]
        public void ProcessRequest_GetsResponseFromRequest()
        {
            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]");

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            JsonRequest testObject = mock.Create<JsonRequest>();

            testObject.Submit<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            submitter.Verify(s => s.GetResponse());
        }

        [Fact]
        public void ProcessRequest_ReadsResultFromResponseReader()
        {
            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]");

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            mock.Mock<IHttpRequestSubmitterFactory>()
                .Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(submitter.Object);

            JsonRequest testObject = mock.Create<JsonRequest>();

            testObject.Submit<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            response.Verify(r => r.ReadResult());
        }
        
        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}