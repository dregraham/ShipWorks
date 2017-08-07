using System;
using System.Net;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Stores.Platforms.Jet;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Jet
{
    public class JsonRequestTest: IDisposable
    {
        private readonly AutoMock mock;

        public JsonRequestTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
        }

        [Fact]
        public void ProcessRequest_LogsRequest()
        {
            var response = mock.CreateMock<IHttpResponseReader>();
            response.Setup(r => r.ReadResult())
                .Returns("[{\"Foo\":\"Bar\"},{\"Foo\":\"Bar\"}]");

            var submitter = mock.CreateMock<IHttpRequestSubmitter>();
            submitter.Setup(s => s.GetResponse())
                .Returns(response.Object);

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JsonRequest>();
            testObject.ProcessRequest<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            logEntry.Verify(e => e.LogRequest(submitter.Object), Times.Once());
        }

        [Fact]
        public void ProcessRequestLogsResponse_WhenWebRequestSucessful()
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

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JsonRequest>();
            testObject.ProcessRequest<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

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

            Mock<IApiLogEntry> logEntry = CreateLogger();

            var testObject = mock.Create<JsonRequest>();

            Assert.Throws<WebException>( () =>
                testObject.ProcessRequest<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object));

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

            testObject.ProcessRequest<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

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

            testObject.ProcessRequest<object>("ProcessRequest", ApiLogSource.Jet, submitter.Object);

            response.Verify(r => r.ReadResult());
        }

        private Mock<IApiLogEntry> CreateLogger()
        {
            var logEntry = mock.CreateMock<IApiLogEntry>();

            var apiLogEntryFactory = mock.CreateMock<Func<ApiLogSource, string, IApiLogEntry>>();
            apiLogEntryFactory.Setup(f => f(ApiLogSource.Jet, "ProcessRequest"))
                .Returns(logEntry.Object);
            mock.Provide(apiLogEntryFactory.Object);
            return logEntry;
        }

        public void Dispose()
        {
            mock?.Dispose();
        }
    }
}