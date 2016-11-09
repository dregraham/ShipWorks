using Autofac.Extras.Moq;
using Interapptive.Shared.Net;
using Moq;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping.Carriers.FedEx.Api.Fims;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Tests.Shared.EntityBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Xunit;

namespace ShipWorks.Tests.Shipping.Carriers.FedEx.Api.Fims
{
    public class FimsWebClientTest : IDisposable
    {
        private AutoMock mock;
        private string label = "myLabelData";
        private byte[] labelBytes;
        
        Mock<IHttpRequestSubmitterFactory> submitterFactory;

        // fields used in response:
        private string base64Label;
        private const string parcelId = "MyParcelId";
        private const string trackingNo = "MyTrackingNumber";
        private const string responseFormat = "Z";

        public FimsWebClientTest()
        {
            mock = AutoMock.GetLoose();

            labelBytes = Encoding.UTF8.GetBytes(label);
            base64Label = Convert.ToBase64String(labelBytes);

            var responseReader = mock.MockRepository.Create<IHttpResponseReader>();
            responseReader.Setup(r => r.ReadResult()).Returns(GetResponse);

            var requestSubmitter = mock.MockRepository.Create<HttpRequestSubmitter>();
            requestSubmitter.Setup(s => s.GetResponse()).Returns(responseReader.Object);

            submitterFactory = mock.Mock<IHttpRequestSubmitterFactory>();
            submitterFactory.Setup(f => f.GetHttpTextPostRequestSubmitter(It.IsAny<string>(), string.Empty))
                .Returns(requestSubmitter.Object);
        }

        [Fact]
        public void Ship_ThrowsArgumentNullException_WhenFimsShipRequestIsNull()
        {
            var testObject = mock.Create<FimsWebClient>();
            Assert.Throws<ArgumentNullException>(() => testObject.Ship(null));
        }

        [Fact]
        public void Ship_CustCodeIsUsername()
        {
            string username = "userName";
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);
            request.SetupGet(r => r.Username).Returns(username);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/custCode", username);
        }

        [Fact]
        public void Ship_ServiceIdIsPassword()
        {
            string password = "password";
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);
            request.SetupGet(r => r.Password).Returns(password);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/serviceId", password);
        }

        [Fact]
        public void Ship_LabelSourceIsSetCorrectly()
        {
            string labelSource = "5";
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);
            
            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/labelSource", labelSource);
        }

        [Theory]
        [InlineData(ThermalLanguage.EPL, "I")]
        [InlineData(ThermalLanguage.ZPL, "Z")]
        [InlineData(ThermalLanguage.None, "I")]
        public void Ship_RequestResponseFormat(ThermalLanguage language, string expectedResponseFormat)
        {
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f=>f.Service, (int) FedExServiceType.FedExFimsMailView))
                .Set(s=>s.RequestedLabelFormat, (int)language)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/responseFormat", expectedResponseFormat);
        }

        [Fact]
        public void Ship_LabelSize()
        {
            string labelSize = "6";
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/labelSize", labelSize);
        }

        [Fact]
        public void Ship_ShipperReference()
        {
            string referenceFims = "FimsRef";
            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView)
                    .Set(f=>f.ReferenceFIMS, referenceFims))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipperReference", referenceFims);
        }

        [Theory]
        [InlineData(FedExServiceType.FedExFimsMailView, 4.3, 399, 41)]
        [InlineData(FedExServiceType.FedExFimsMailView, 4.4, 399, 42)]
        [InlineData(FedExServiceType.FedExFimsMailView, 4.3, 400, 42)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite, 4.3, 399, 51)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite, 4.4, 399, 22)]
        [InlineData(FedExServiceType.FedExFimsMailViewLite, 4.3, 400, 22)]
        [InlineData(FedExServiceType.FedExFimsStandard, 4.3, 399, 31)]
        [InlineData(FedExServiceType.FedExFimsStandard, 4.4, 399, 22)]
        [InlineData(FedExServiceType.FedExFimsStandard, 4.3, 400, 22)]
        [InlineData(FedExServiceType.FedExFimsPremium, 4.3, 399, 21)]
        [InlineData(FedExServiceType.FedExFimsPremium, 4.4, 399, 22)]
        [InlineData(FedExServiceType.FedExFimsPremium, 4.3, 400, 22)]
        public void Ship_LabelType(FedExServiceType serviceType, double totalWeight, decimal customsValue, int responseFormat)
        {
            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) serviceType))
               .Set(s=>s.TotalWeight, totalWeight)
               .Set(s=>s.CustomsValue, customsValue)
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/labelType", responseFormat);
        }

        [Theory]
        [InlineData(FedExCommercialInvoicePurpose.Gift, "G")]
        [InlineData(FedExCommercialInvoicePurpose.NotSold, "X")]
        [InlineData(FedExCommercialInvoicePurpose.Personal, "X")]
        [InlineData(FedExCommercialInvoicePurpose.Repair, "X")]
        [InlineData(FedExCommercialInvoicePurpose.Sample, "S")]
        [InlineData(FedExCommercialInvoicePurpose.Sold, "X")]
        public void Ship_Declaration(FedExCommercialInvoicePurpose purpose, string expectedDeclaration)
        {
            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage()
                .Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium)
                .Set(f => f.CommercialInvoicePurpose, (int) purpose))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/declaration", expectedDeclaration);
        }

        [Fact]
        public void Ship_Weight()
        {
            var expectedValue = 42;

            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium))
               .Set(s=>s.TotalWeight, expectedValue)
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/pkgWeight", expectedValue);
        }

        [Fact]
        public void Ship_Length()
        {
            var expectedValue = 5;

            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage(p=>p.Set(s=>s.DimsLength, expectedValue))
               .Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/pkgLength", expectedValue);
        }

        [Fact]
        public void Ship_Width()
        {
            var expectedValue = 51;

            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage(p => p.Set(s => s.DimsWidth, expectedValue))
               .Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/pkgWidth", expectedValue);
        }

        [Fact]
        public void Ship_Height()
        {
            var expectedValue = 15;

            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage(p => p.Set(s => s.DimsHeight, expectedValue))
               .Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/pkgHeight", expectedValue);
        }

        [Fact]
        public void Ship_AirWayBill()
        {
            var expectedValue = "123456789012";

            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage()
               .Set(f => f.FimsAirWaybill, expectedValue)
               .Set(f => f.Service, (int) FedExServiceType.FedExFimsPremium))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/airWaybill", expectedValue);
        }

        #region "Origin Tests"
        [Fact]
        public void Ship_ShipperName()
        {
            var firstName = "Homer";
            var middleName = "J";
            var lastName = "Simpson";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginFirstName, firstName)
                    .Set(f => f.OriginMiddleName, middleName)
                    .Set(f => f.OriginLastName, lastName)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/name", $"{firstName} {middleName} {lastName}");
        }

        [Fact]
        public void Ship_ShipperCompany()
        {
            var company = "ShipTwerks";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginCompany, company)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/company", company);
        }

        [Fact]
        public void Ship_ShipperAddress1()
        {
            var expectedValue = "address 1";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginStreet1, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/address1", expectedValue);
        }

        [Fact]
        public void Ship_ShipperAddress2()
        {
            var expectedValue = "address 2";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginStreet2, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/address2", expectedValue);
        }

        [Fact]
        public void Ship_ShipperCity()
        {
            var expectedValue = "city";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginCity, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/city", expectedValue);
        }

        [Fact]
        public void Ship_ShipperState()
        {
            var expectedValue = "ST";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginStateProvCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/state", expectedValue);
        }

        [Fact]
        public void Ship_ShipperZip()
        {
            var expectedValue = "456789";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginPostalCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/zipCode", expectedValue);
        }

        [Fact]
        public void Ship_ShipperCountry()
        {
            var expectedValue = "CA";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginCountryCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/country", expectedValue);
        }

        [Fact]
        public void Ship_ShipperPhone()
        {
            var expectedValue = "314 555 1212";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginPhone, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/phone", expectedValue);
        }

        [Fact]
        public void Ship_ShipperEmail()
        {
            var expectedValue = "kcroke@shipworks.com";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.OriginEmail, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/shipper/email", expectedValue);
        }
#endregion "Origin Tests"

        #region "ShipTo Tests"
        [Fact]
        public void Ship_ShipToName()
        {
            var firstName = "Homer";
            var middleName = "J";
            var lastName = "Simpson";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipFirstName, firstName)
                    .Set(f => f.ShipMiddleName, middleName)
                    .Set(f => f.ShipLastName, lastName)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/name", $"{firstName} {middleName} {lastName}");
        }

        [Fact]
        public void Ship_ShipToCompany()
        {
            var company = "ShipTwerks";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipCompany, company)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/company", company);
        }

        [Fact]
        public void Ship_ShipToAddress1()
        {
            var expectedValue = "address 1";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipStreet1, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/address1", expectedValue);
        }

        [Fact]
        public void Ship_ShipToAddress2()
        {
            var expectedValue = "address 2";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipStreet2, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/address2", expectedValue);
        }

        [Fact]
        public void Ship_ShipToCity()
        {
            var expectedValue = "city";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipCity, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/city", expectedValue);
        }

        [Fact]
        public void Ship_ShipToState()
        {
            var expectedValue = "ST";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipStateProvCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/state", expectedValue);
        }

        [Fact]
        public void Ship_ShipToZip()
        {
            var expectedValue = "456789";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipPostalCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/zipCode", expectedValue);
        }

        [Fact]
        public void Ship_ShipToCountry()
        {
            var expectedValue = "CA";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipCountryCode, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/country", expectedValue);
        }

        [Fact]
        public void Ship_ShipToPhone()
        {
            var expectedValue = "314 555 1212";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipPhone, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/phone", expectedValue);
        }

        [Fact]
        public void Ship_ShipToEmail()
        {
            var expectedValue = "kcroke@shipworks.com";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage().Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                    .Set(f => f.ShipEmail, expectedValue)
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/consignee/email", expectedValue);
        }

        #endregion "ShipTo Tests"

        #region "Customs Tests"

        [Fact]
        public void Ship_CustomsDescription()
        {
            string expectedValue = "ciDesc";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem(c=>c.Set(ci=>ci.Description = expectedValue))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/description", expectedValue);
        }

        [Fact]
        public void Ship_CustomsValue()
        {
            decimal expectedValue = 44.5M;

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem(c => c.Set(ci => ci.UnitValue = expectedValue))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/value", expectedValue);
        }

        [Fact]
        public void Ship_CustomsCurrency()
        {
            string expectedValue = "USD";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem()
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/currency", expectedValue);
        }

        [Fact]
        public void Ship_CustomsWeight()
        {
            double expectedValue = 500;

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem(c => c.Set(ci => ci.Weight = expectedValue))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/weight", expectedValue.ToString());
        }

        [Fact]
        public void Ship_CustomsTariffNumber()
        {
            string expectedValue = "ABCD";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem(c => c.Set(ci => ci.HarmonizedCode = expectedValue))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/tariffNo", expectedValue);
        }

        [Fact]
        public void Ship_CustomsOriginCountry()
        {
            string expectedValue = "USSR";

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem(c => c.Set(ci => ci.CountryOfOrigin = expectedValue))
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateRequest("//labelRequest/commodities/commodity/originCountry", expectedValue);
        }

        [Fact]
        public void Ship_OneCommodityNodeAdded_WhenOneCustomsItem()
        {
            int expectedValue = 1;

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem()
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateCountRequest("//labelRequest/commodities/commodity", expectedValue);
        }

        [Fact]
        public void Ship_TwoCommodityNodesAdded_WhenTwoCustomsItems()
        {
            int expectedValue = 2;

            var shipment = Create.Shipment()
                .AsFedEx(x => x.WithPackage()
                    .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
                .WithCustomsItem()
                .WithCustomsItem()
                .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            testObject.Ship(request.Object);

            ValidateCountRequest("//labelRequest/commodities/commodity", expectedValue);
        }

        #endregion "Customs Tests"

        #region "Response Tests"

        [Fact]
        public void Ship_ParcelIdSetFromResponse()
        {
            IFimsShipResponse response = Ship();

            Assert.Equal(parcelId, response.ParcelID);
        }

        [Fact]
        public void Ship_TrackingNumberSetFromResponse()
        {
            IFimsShipResponse response = Ship();

            Assert.Equal(trackingNo, response.TrackingNumber);
        }

        [Fact]
        public void Ship_ResponseFormatSetFromResponse()
        {
            IFimsShipResponse response = Ship();

            Assert.Equal(responseFormat, response.LabelFormat);
        }

        [Fact]
        public void Ship_LabelDataSentFromResponse()
        {
            IFimsShipResponse response = Ship();

            Assert.Equal(labelBytes, response.LabelData);
        }

        private IFimsShipResponse Ship()
        {
            var shipment = Create.Shipment()
               .AsFedEx(x => x.WithPackage()
                   .Set(f => f.Service, (int) FedExServiceType.FedExFimsMailView))
               .Build();

            var request = mock.MockRepository.Create<IFimsShipRequest>();
            request.SetupGet(r => r.Shipment).Returns(shipment);

            var testObject = mock.Create<FimsWebClient>();
            var response = testObject.Ship(request.Object);
            return response;
        }

        #endregion "Response Tests"

        public string GetResponse()
        {
            return 
                "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\">" +
                "  <SOAP-ENV:Header />" +
                "  <SOAP-ENV:Body>" +
                "    <labelResponse xmlns=\"http://www.fimsform.com\">" +
               $"      <responseFormat>{responseFormat}</responseFormat>" +
                "      <labelSize>6</labelSize>" +
               $"      <parcelId>{parcelId}</parcelId>" +
               $"      <trackingNo>{trackingNo}</trackingNo>" +
                "      <responseCode>1</responseCode>" +
               $"      <attached_label>{base64Label}</attached_label>" +
                "    </labelResponse>" +
                "  </SOAP-ENV:Body>" +
                "</SOAP-ENV:Envelope>";
        }


        private void ValidateRequest(string xpath, int expectedValue) => ValidateRequest(xpath, expectedValue.ToString());

        private void ValidateRequest(string xpath, decimal expectedValue) => ValidateRequest(xpath, expectedValue.ToString());

        private void ValidateCountRequest(string xpath, int expectedCount)
        {
            submitterFactory
                .Verify(x => x.GetHttpTextPostRequestSubmitter(
                    It.Is<string>(xml => ValidateCountRequest(xml, xpath, expectedCount)),
                    string.Empty), Times.Once);
        }

        private bool ValidateCountRequest(string xml, string xpath, int expectedCount)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNodeList nodes = doc.SelectNodes(xpath);
            return (nodes?.Count ?? 0) == expectedCount;
        }

        private void ValidateRequest(string xpath, string expectedValue)
        {
            submitterFactory
                .Verify(x => x.GetHttpTextPostRequestSubmitter(
                    It.Is<string>(s => ValidateRequest(s, xpath, expectedValue)),
                    string.Empty), Times.Once);
        }

        private bool ValidateRequest(string xml, string xpath, string expectedValue)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode node = doc.SelectSingleNode(xpath);
            return node != null && expectedValue.Equals(node.InnerXml, StringComparison.Ordinal);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    } 
}
