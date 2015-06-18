using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping;
using System.Xml;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Shipping.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
    public class ShippingRequestTest
    {
        private Credentials credentials;
        
        private INeweggRequest allSuccessRequest;
        private INeweggRequest successAndFailureRequest;
        private INeweggRequest allFailuresRequest;
        private INeweggRequest errorRequest;
        
        private string sellerId = "ABC123";
        private long orderNumber = 159243598;

        private Shipment shipment;
        private ShippingRequest testObject;

        [TestInitialize]
        public void Initialize()
        {
            credentials = new Credentials(sellerId, string.Empty, NeweggChannelType.US);
            InitializeShipment();

            string errorResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <Errors>
                      <Error>
                        <Code>SO020</Code>
                        <Message>There is a package or packages without shipping information in this shipment.</Message>
                      </Error>
                    </Errors>";

            string allSuccessfulResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<UpdateOrderStatusInfo>
  <IsSuccess>true</IsSuccess>
  <PackageProcessingSummary>
    <TotalPackageCount>2</TotalPackageCount>
    <SuccessCount>2</SuccessCount>
    <FailCount>0</FailCount>
  </PackageProcessingSummary>
  <Result>
    <OrderNumber>{0}</OrderNumber>
    <SellerID>{1}</SellerID>
    <OrderStatus>Shipped</OrderStatus>
    <Shipment>
      <PackageList>
        <Package>
          <TrackingNumber>unitTestSuccess1</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>true</ProcessStatus>
          <ProcessResult>Success</ProcessResult>
          <ItemList>
            <ItemDes>
              <NeweggItemNumber>9SIA0060845543</NeweggItemNumber>
              <SellerPartNumber>A006ZX-35833</SellerPartNumber>
              <ShippedQty>1</ShippedQty>
            </ItemDes>
          </ItemList>
        </Package>
        <Package>
          <TrackingNumber>unitTestSuccess2</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>true</ProcessStatus>
          <ProcessResult>Success</ProcessResult>
          <ItemList>
            <ItemDes>
              <NeweggItemNumber>9SIA0060845543</NeweggItemNumber>
              <SellerPartNumber>A006ZX-35833</SellerPartNumber>
              <ShippedQty>1</ShippedQty>
            </ItemDes>
          </ItemList>
        </Package>
      </PackageList>
    </Shipment>
  </Result>
</UpdateOrderStatusInfo>", orderNumber, sellerId);

            string successAndFailureResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<UpdateOrderStatusInfo>
  <IsSuccess>true</IsSuccess>
  <PackageProcessingSummary>
    <TotalPackageCount>2</TotalPackageCount>
    <SuccessCount>1</SuccessCount>
    <FailCount>1</FailCount>
  </PackageProcessingSummary>
  <Result>
    <OrderNumber>{0}</OrderNumber>
    <SellerID>{1}</SellerID>
    <OrderStatus>Shipped</OrderStatus>
    <Shipment>
      <PackageList>
        <Package>
          <TrackingNumber>unitTestSuccess1</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>true</ProcessStatus>
          <ProcessResult>Success</ProcessResult>
          <ItemList>
            <ItemDes>
              <NeweggItemNumber>9SIA0060845543</NeweggItemNumber>
              <SellerPartNumber>A006ZX-35833</SellerPartNumber>
              <ShippedQty>1</ShippedQty>
            </ItemDes>
          </ItemList>
        </Package>
        <Package>
          <TrackingNumber>unitTestFailed1</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>false</ProcessStatus>
          <ProcessResult>Simulating a failure for unit testing.</ProcessResult>
          <ItemList/>
        </Package>
      </PackageList>
    </Shipment>
  </Result>
</UpdateOrderStatusInfo>", orderNumber, sellerId); ;

            string allFailuresResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<UpdateOrderStatusInfo>
  <IsSuccess>true</IsSuccess>
  <PackageProcessingSummary>
    <TotalPackageCount>2</TotalPackageCount>
    <SuccessCount>0</SuccessCount>
    <FailCount>2</FailCount>
  </PackageProcessingSummary>
  <Result>
    <OrderNumber>{0}</OrderNumber>
    <SellerID>{1}</SellerID>
    <OrderStatus>Shipped</OrderStatus>
    <Shipment>
      <PackageList>
        <Package>
          <TrackingNumber>unitTestFailure1</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>false</ProcessStatus>
          <ProcessResult>Simulating a failure for unit testing.</ProcessResult>
          <ItemList>
            <ItemDes>
              <NeweggItemNumber>9SIA0060845543</NeweggItemNumber>
              <SellerPartNumber>A006ZX-35833</SellerPartNumber>
              <ShippedQty>1</ShippedQty>
            </ItemDes>
          </ItemList>
        </Package>
        <Package>
          <TrackingNumber>unitTestFailure2</TrackingNumber>
          <ShipDate>2012-02-10T15:30:01</ShipDate>
          <ProcessStatus>false</ProcessStatus>
          <ProcessResult>Simulating a failure for unit testing.</ProcessResult>
          <ItemList/>
        </Package>
      </PackageList>
    </Shipment>
  </Result>
</UpdateOrderStatusInfo>", orderNumber, sellerId); ;

            errorRequest = new Mocked.MockedNeweggRequest(errorResponse);
            allSuccessRequest = new Mocked.MockedNeweggRequest(allSuccessfulResponse);
            successAndFailureRequest = new Mocked.MockedNeweggRequest(successAndFailureResponse);
            allFailuresRequest = new Mocked.MockedNeweggRequest(allFailuresResponse);
        }

        private void InitializeShipment()
        {
            this.shipment = new Shipment();
            shipment.Header = new ShipmentHeader { OrderNumber = this.orderNumber, SellerId = this.sellerId };
            
            ShipmentPackage package = new ShipmentPackage();
            package.ShipCarrier = "UPS";
            package.ShipDateInPacificStandardTime = System.TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            package.ShipFromAddress1 = "123 Main Street";
            package.ShipFromCity = "St.Louis";
            package.ShipFromCountry = "USA";
            package.ShipFromState = "MO";
            package.ShipFromZipCode = "63102";
            package.ShipService = "Ground";
            package.Items.Add(new ShippedItem { NeweggItemNumber = "1234ABCD", QuantityShipped = 1, SellerPartNumber = "WXYZ7890" });
            
            shipment.Packages.Add(package);
        }

        [TestMethod]
        [ExpectedException(typeof(NeweggException))]
        public void Ship_ThrowsNeweggException_WhenErrorResponseIsReceived_Test()
        {
            testObject = new ShippingRequest(credentials, errorRequest);
            testObject.Ship(shipment);
        }

        [TestMethod]
        public void Ship_ReturnsCorrectPackageSummary_WhenAllSuccessful_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.AreEqual(2, shippingResult.PackageSummary.TotalPackages);
            Assert.AreEqual(2, shippingResult.PackageSummary.SuccessCount);
            Assert.AreEqual(0, shippingResult.PackageSummary.FailedCount);
        }

        [TestMethod]
        public void Ship_ReturnsCorrectPackageSummary_WhenContainingSuccessesAndFailures_Test()
        {
            testObject = new ShippingRequest(credentials, successAndFailureRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.AreEqual(2, shippingResult.PackageSummary.TotalPackages);
            Assert.AreEqual(1, shippingResult.PackageSummary.SuccessCount);
            Assert.AreEqual(1, shippingResult.PackageSummary.FailedCount);
        }

        [TestMethod]
        public void Ship_ReturnsCorrectPackageSummary_WhenAllFailures_Test()
        {
            testObject = new ShippingRequest(credentials, allFailuresRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.AreEqual(2, shippingResult.PackageSummary.TotalPackages);
            Assert.AreEqual(0, shippingResult.PackageSummary.SuccessCount);
            Assert.AreEqual(2, shippingResult.PackageSummary.FailedCount);
        }

        [TestMethod]
        public void Ship_ContainsPackageList_WhenSuccessResponse_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.IsTrue(shippingResult.Detail.Shipment.Packages.Count > 0);
        }

        [TestMethod]
        public void Ship_ContainsItemList_WhenSuccessResponse_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.IsTrue(shippingResult.Detail.Shipment.Packages[0].Items.Count > 0);
        }

        [TestMethod]
        public void Ship_ContainsSellerId_WhenSuccessResponse_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.AreEqual(sellerId, shippingResult.Detail.SellerId);
        }

        [TestMethod]
        public void Ship_ContainsOrderNumber_WhenSuccessResponse_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            ShippingResult shippingResult = testObject.Ship(shipment);

            Assert.AreEqual(this.orderNumber, shippingResult.Detail.OrderNumber);
        }
        
        [TestMethod]
        public void Ship_FormatsUrlWithOrderNumberAndSellerId_Test()
        {
            string expectedUrl = string.Format("https://api.newegg.com/marketplace/ordermgmt/orderstatus/orders/{0}?sellerid={1}", shipment.Header.OrderNumber, sellerId);

            testObject = new ShippingRequest(credentials, allSuccessRequest);

            testObject.Ship(shipment);

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            Assert.AreEqual(expectedUrl, ((Mocked.MockedNeweggRequest)allSuccessRequest).Url);
        }

        [TestMethod]
        public void Ship_BuildsRequestBody_WithActionValueOfTwo_Test()
        {
            const int expectedActionValue = 2;
            testObject = new ShippingRequest(credentials, allSuccessRequest);

            testObject.Ship(shipment);

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(((Mocked.MockedNeweggRequest)allSuccessRequest).Body);
            int actualActionValue = int.Parse(requestXml.SelectSingleNode("UpdateOrderStatus/Action").InnerText);

            Assert.AreEqual(expectedActionValue, actualActionValue);
        }

        [TestMethod]
        public void Ship_BuildsRequestBody_WithSerializedShipmentXml_Test()
        {
            string expectedValue = SerializationUtility.SerializeToXml(shipment);
            expectedValue = expectedValue.Replace("<ItemDes>", "<Item>");
            expectedValue = expectedValue.Replace("</ItemDes>", "</Item>");

            XmlDocument document = new XmlDocument();
            document.LoadXml(expectedValue);

            XmlNodeList packageNodes = document.SelectNodes("/Shipment/PackageList/Package");
            foreach (XmlNode node in packageNodes)
            {
                XmlNode nodeToRemove = node.SelectSingleNode("ProcessStatus");
                if (nodeToRemove != null)
                {
                    node.RemoveChild(nodeToRemove);
                }
            }

            expectedValue = document.OuterXml;

            testObject = new ShippingRequest(credentials, allSuccessRequest);
            testObject.Ship(shipment);

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(((Mocked.MockedNeweggRequest)allSuccessRequest).Body);
            string actualValue = requestXml.SelectSingleNode("UpdateOrderStatus/Value").InnerText.Trim();

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void Ship_UsesPutRequestMethod_Test()
        {
            testObject = new ShippingRequest(credentials, allSuccessRequest);
            testObject.Ship(shipment);

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request          
            Assert.AreEqual(HttpVerb.Put, ((Mocked.MockedNeweggRequest)allSuccessRequest).Method);
        }

        [TestMethod]
        [Ignore]
        public void Ship_ReturnsShippingResults_WhenShippingAnOrderWithNeweggAPI_IntegrationTest()
        {
        //    // We're going to try to bounce the request off of the Newegg API, so setup 
        //    // the test object to use a "live" NeweggHttpRequest and an order setup in
        //    // our sandbox seller account, and use the sandbox seller account credentials

        //    // TODO: Plug in an unshipped order number and build the appropriate item list to test
        //    Shipment shipment = new Shipment { ... };
        //    Credentials credentials = new Credentials("A09V", "E09799F3-A8FD-46E0-989F-B8587A1817E0");
        //    testObject = new ShippingRequest(credentials, new NeweggHttpRequest());

        //    ShippingResult shippingResult = testObject.Ship(shipment);

        //    Assert.IsTrue(shippingResult.IsSuccessful);
        //    Assert.AreEqual(shipment.Header.OrderNumber, shippingResult.Detail.OrderNumber);
        //    Assert.AreEqual(shipment.Header.SellerId, shippingResult.Detail.SellerId);            
            Assert.Inconclusive("Need an unshipped order to run this integration test.");
        }
    }
}
