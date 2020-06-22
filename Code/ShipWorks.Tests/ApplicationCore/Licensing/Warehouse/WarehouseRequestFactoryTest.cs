using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net.RestSharp;
using Moq;
using RestSharp;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Common.Net;
using ShipWorks.Stores.Platforms.Amazon.WebServices.Associates;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Tests.ApplicationCore.Licensing.Warehouse
{
    public class WarehouseRequestFactoryTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IRestRequest> restRequest;

        public WarehouseRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            restRequest = mock.FromFactory<IRestRequestFactory>()
                .Mock(f => f.Create("endpoint", Method.POST));
        }

        [Fact]
        public void Create_InitializesRestRequestProperly()
        {
            var testObject = mock.Create<WarehouseRequestFactory>();

            testObject.Create("endpoint", Method.POST, null);

            mock.Mock<IRestRequestFactory>().Verify(f => f.Create("endpoint", Method.POST), Times.Once);
        }

        [Fact]
        public void Create_InitializesJsonSerializer()
        {
            var testObject = mock.Create<WarehouseRequestFactory>();
            var result = testObject.Create("endpoint", Method.POST, null);

            restRequest.VerifySet(r => r.JsonSerializer = It.IsAny<RestSharpJsonNetSerializer>());
        }

        [Fact]
        public void Create_InitializesRequestFormat()
        {
            var testObject = mock.Create<WarehouseRequestFactory>();
            var result = testObject.Create("endpoint", Method.POST, null);

            restRequest.VerifySet(r => r.RequestFormat = DataFormat.Json);
        }

        [Fact]
        public void Create_PayloadIsAdded()
        {
            var testObject = mock.Create<WarehouseRequestFactory>();
            var payload = new object();
            var result = testObject.Create("endpoint", Method.POST, payload);

            restRequest.Verify(r => r.AddJsonBody(payload), Times.Once);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
