using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Serialization;
using ShipWorks.Common.Net;
using ShipWorks.Data;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Products.Warehouse;
using ShipWorks.Products.Warehouse.DTO;
using ShipWorks.Tests.Shared;
using Xunit;
using static ShipWorks.Tests.Shared.ExtensionMethods.ParameterShorteners;

namespace ShipWorks.Products.Tests.Warehouse.DTO
{
    public class WarehouseProductRequestFactoryTest
    {
        private readonly AutoMock mock;
        private readonly WarehouseProductRequestFactory testObject;

        public WarehouseProductRequestFactoryTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            mock.Mock<IConfigurationData>()
                .Setup(x => x.FetchReadOnly())
                .Returns(new ConfigurationEntity { WarehouseID = "ABC123" });

            testObject = mock.Create<WarehouseProductRequestFactory>();
        }

        [Fact]
        public void Create_WithPayload_SetsWarehouseIdFromConfiguration()
        {
            var payload = mock.Mock<IWarehouseProductRequestData>();

            testObject.Create("/foo", Method.GET, payload.Object);

            payload.VerifySet(x => x.WarehouseId = "ABC123");
        }

        [Fact]
        public void Create_WithPayload_DelegatesToRestRequestFactory()
        {
            testObject.Create("/foo", Method.PATCH, mock.Build<IWarehouseProductRequestData>());

            mock.Mock<IRestRequestFactory>()
                .Verify(x => x.Create("/foo", Method.PATCH));
        }

        [Fact]
        public void Create_WithPayload_ConfiguresRequest()
        {
            var payload = mock.Build<IWarehouseProductRequestData>();
            var request = mock.Mock<IRestRequest>();
            mock.Mock<IRestRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>()))
                .Returns(request.Object);

            testObject.Create("/foo", Method.PATCH, payload);

            request.VerifySet(x => x.RequestFormat = DataFormat.Json);
            request.VerifySet(x => x.JsonSerializer = It.Is<IRestSerializer>(s => ValidateSerializer(s)));
            request.Verify(x => x.AddJsonBody(payload));
        }

        [Fact]
        public void Create_WithPayload_ReturnsRequest()
        {
            var request = mock.Build<IRestRequest>();
            mock.Mock<IRestRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>()))
                .Returns(request);

            var result = testObject.Create("/foo", Method.PATCH, mock.Build<IWarehouseProductRequestData>());

            Assert.Equal(request, result);
        }

        [Fact]
        public void Create_DelegatesToRestRequestFactory()
        {
            testObject.Create("/foo", Method.PATCH, mock.Build<IWarehouseProductRequestData>());

            mock.Mock<IRestRequestFactory>()
                .Verify(x => x.Create("/foo", Method.PATCH));
        }

        [Fact]
        public void Create_ConfiguresRequest()
        {
            var payload = mock.Build<IWarehouseProductRequestData>();
            var request = mock.Mock<IRestRequest>();
            mock.Mock<IRestRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>()))
                .Returns(request.Object);

            testObject.Create("/foo", Method.PATCH, payload);

            request.VerifySet(x => x.RequestFormat = DataFormat.Json);
            request.VerifySet(x => x.JsonSerializer = It.Is<IRestSerializer>(s => ValidateSerializer(s)));
        }

        [Fact]
        public void Create_ReturnsRequest()
        {
            var request = mock.Build<IRestRequest>();
            mock.Mock<IRestRequestFactory>()
                .Setup(x => x.Create(AnyString, It.IsAny<Method>()))
                .Returns(request);

            var result = testObject.Create("/foo", Method.PATCH);

            Assert.Equal(request, result);
        }

        private bool ValidateSerializer(IRestSerializer restSerializer) =>
            restSerializer is RestSharpJsonNetSerializer serializer &&
                serializer.Settings.ContractResolver is DefaultContractResolver resolver &&
                resolver.NamingStrategy is CamelCaseNamingStrategy &&
                !resolver.NamingStrategy.OverrideSpecifiedNames;
    }
}
