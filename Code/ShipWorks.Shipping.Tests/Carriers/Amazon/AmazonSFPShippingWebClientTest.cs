using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using Moq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.SFP;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api;
using ShipWorks.Shipping.Carriers.Amazon.SFP.Api.DTOs;
using ShipWorks.Stores.Platforms.Amazon.Mws;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Shipping.Tests.Carriers.Amazon
{
    public class AmazonSFPShippingWebClientTest : IDisposable
    {
        private TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>("testing");
        readonly AutoMock mock;
        readonly ShipmentRequestDetails request;
        readonly CreateShipmentResponse response;
        readonly Mock<IAmazonMwsWebClientSettings> settings;
        private Uri proxy;
        private readonly AmazonSFPShipmentEntity shipment;

        public AmazonSFPShippingWebClientTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();

            request = new ShipmentRequestDetails
            {
                ItemList = new List<Item>(),
                ShipFromAddress = new Address(),
                PackageDimensions = new PackageDimensions(),
                ShippingServiceOptions = new ShippingServiceOptions
                {
                    DeclaredValue = new DeclaredValue(),
                    LabelFormat = null
                }
            };

            response = new CreateShipmentResponse
            {
                CreateShipmentResult = new CreateShipmentResult
                {
                    AmazonShipment = new AmazonShipment
                    {
                        Label = new Label
                        {
                            FileContents = new FileContents()
                        }
                    }
                }
            };

            shipment = new AmazonSFPShipmentEntity()
            {
                ShippingServiceID = "test"
            };

            var responseReader = mock.Mock<IHttpResponseReader>();
            responseReader.Setup(x => x.ReadResult()).Returns(() => SerializationUtility.SerializeToXml(response));

            var submitter = mock.Mock<IHttpVariableRequestSubmitter>();
            submitter.SetupGet(x => x.Variables).Returns(new HttpVariableCollection());
            submitter.SetupGet(x => x.Uri).Returns(new Uri("http://www.example.com"));
            submitter.Setup(x => x.GetResponse()).Returns(responseReader);

            mock.FromFactory<IEncryptionProviderFactory>()
                .Mock(x => x.CreateSecureTextEncryptionProvider("Interapptive"))
                .Setup(x => x.Decrypt(It.IsAny<string>()))
                .Returns("FooKey");

            proxy = new Uri("http://www.example.com/proxy");
            settings = mock.CreateMock<IAmazonMwsWebClientSettings>();
            settings.SetupGet(x => x.Endpoint).Returns("http://www.example.com");
            settings.Setup(x => x.GetApiNamespace(It.IsAny<AmazonMwsApiCall>())).Returns(XNamespace.Get("https://mws.amazonservices.com/MerchantFulfillment/2015-06-01"));
            settings.SetupGet(s => s.ProxyEndpoint).Returns(proxy);

            var settingsFactory = mock.Mock<IAmazonMwsWebClientSettingsFactory>();

            settingsFactory.Setup(f => f.Create(It.IsAny<AmazonSFPShipmentEntity>())).Returns(settings);
        }

        [Fact]
        public void CreateShipment_ThrowsAmazonShipperExceptionWhen_ShipmentResponseIsNull()
        {
            mock.Mock<IHttpResponseReader>()
                   .Setup(x => x.ReadResult()).Returns("<Foo></Foo>");

            var testObject = mock.Create<AmazonSFPShippingWebClient>();

            Assert.Throws<AmazonSFPShippingException>(() => testObject.CreateShipment(request, shipment, telemetricResult));
        }

        [Fact]
        public void CreateShipment_ReturnsShipment_WhenResponseIsValid()
        {
            var testObject = mock.Create<AmazonSFPShippingWebClient>();

            var result = testObject.CreateShipment(request, shipment, telemetricResult);

            Assert.NotNull(result);
        }

        [Fact]
        public void CreateShipment_UsesProxy_WhenShipping()
        {
            var testObject = mock.Create<AmazonSFPShippingWebClient>();

            testObject.CreateShipment(request, shipment, telemetricResult);
            
            mock.Mock<IHttpVariableRequestSubmitter>()
                .VerifySet(r=>r.Uri=proxy);
        }

        public void Dispose() => mock.Dispose();
    }
}
