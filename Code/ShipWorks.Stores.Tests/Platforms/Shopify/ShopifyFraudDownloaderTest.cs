using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Extras.Moq;
using Interapptive.Shared.Utility;
using Moq;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Shopify;
using ShipWorks.Tests.Shared;
using Xunit;

namespace ShipWorks.Stores.Tests.Platforms.Shopify
{
    public class ShopifyFraudDownloaderTest : IDisposable
    {
        private readonly AutoMock mock;
        private readonly Mock<IShopifyWebClient> webClient;
        private const string TwoRisks_Valid = "{\r\n  \"risks\": [\r\n    {\r\n      \"id\": 284138680,\r\n      \"order_id\": 450789469,\r\n      \"checkout_id\": null,\r\n      \"source\": \"External\",\r\n      \"score\": \"1.0\",\r\n      \"recommendation\": \"investigate\",\r\n      \"display\": true,\r\n      \"cause_cancel\": true,\r\n      \"message\": \"This order was placed from a proxy IP\",\r\n      \"merchant_message\": \"This order was placed from a proxy IP\"\r\n    },\r\n    {\r\n      \"id\": 1066215051,\r\n      \"order_id\": 450789469,\r\n      \"checkout_id\": 901414060,\r\n      \"source\": \"External\",\r\n      \"score\": \"1.0\",\r\n      \"recommendation\": \"cancel\",\r\n      \"display\": true,\r\n      \"cause_cancel\": true,\r\n      \"message\": \"This order came from an anonymous proxy\",\r\n      \"merchant_message\": \"This order came from an anonymous proxy\"\r\n    }\r\n  ]\r\n}";

        public ShopifyFraudDownloaderTest()
        {
            mock = AutoMockExtensions.GetLooseThatReturnsMocks();
            webClient = mock.Mock<IShopifyWebClient>();
        }

        [Theory]
        [InlineData("{ \"risks\": [] }")]
        [InlineData("")]
        public async Task Download_ReturnsZeroPaymentDetails_WhenEmptyResponse(string json)
        {
            var risks = ParseFraudRisks(json);
            webClient.Setup(w => w.GetFraudRisks(It.IsAny<long>())).Returns(risks);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();
            var results = await testObject.Download(webClient.Object, 3);

            Assert.Equal(0, results.Count());
        }

        [Fact]
        public async Task Download_ReturnsZeroPaymentDetails_WhenNullList()
        {
            webClient.Setup(w => w.GetFraudRisks(It.IsAny<long>())).Returns(null as IEnumerable<JToken>);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();
            var results = await testObject.Download(webClient.Object, 3);

            Assert.Equal(0, results.Count());
        }

        [Fact]
        public async Task Download_ReturnsTwoOrderPaymentDetails_WhenTwoRisksInResponse()
        {
            var risks = ParseFraudRisks(TwoRisks_Valid);
            webClient.Setup(w => w.GetFraudRisks(It.IsAny<long>())).Returns(risks);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();
            var results = await testObject.Download(webClient.Object, 3);

            Assert.Equal(2, results.Count());
            Assert.Equal("Investigate", results.First().Label);
            Assert.Equal("This order was placed from a proxy IP", results.First().Value);
            Assert.Equal("Cancel", results.Last().Label);
            Assert.Equal("This order came from an anonymous proxy", results.Last().Value);
        }

        [Fact]
        public async Task Download_ReturnsCorrectValues_WhenMissingSomeParts()
        {
            var risks = ParseFraudRisks("{ \"risks\": [ { \"recommendation\": \"cancel\" }, { \"message\": \"A message.\" }] }");
            webClient.Setup(w => w.GetFraudRisks(It.IsAny<long>())).Returns(risks);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();
            var results = await testObject.Download(webClient.Object, 3);

            Assert.Equal(2, results.Count());
            Assert.Equal("Cancel", results.First().Label);
            Assert.Equal(string.Empty, results.First().Value);
            Assert.Equal(string.Empty, results.Last().Label);
            Assert.Equal("A message.", results.Last().Value);
        }

        [Fact]
        public async Task Download_ReturnsNoOrderPaymentDetails_WhenMissingBothParts()
        {
            var risks = ParseFraudRisks("{ \"risks\": [ { \"id\": \"284138680\" }, { \"id\": \"384138680\" }] }");
            webClient.Setup(w => w.GetFraudRisks(It.IsAny<long>())).Returns(risks);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();
            var results = await testObject.Download(webClient.Object, 3);

            Assert.Equal(0, results.Count());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(2)]
        [InlineData(3)]
        public void Merge_AddsNewPaymentDetails_WhenNoExistingOrderPaymentDetails(int fraudRecords)
        {
            ShopifyOrderEntity order = new ShopifyOrderEntity();
            IEnumerable<OrderPaymentDetailEntity> fraudRisks = CreateOrderPaymentDetailEntities(fraudRecords);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();

            testObject.Merge(order, fraudRisks);

            Assert.Equal(fraudRecords, order.OrderPaymentDetails.Select(opd => new { opd.Label, opd.Value }).Distinct().Count());
        }

        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 1)]
        [InlineData(0, 2, 2)]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 2)]
        [InlineData(2, 1, 2)]
        [InlineData(3, 2, 3)]
        public void Merge_AddsCorrectPaymentDetails(int fraudRecords, int orderPaymentDetailCount, int expectedFinalCount)
        {
            ShopifyOrderEntity order = new ShopifyOrderEntity();
            order.OrderPaymentDetails.AddRange(CreateOrderPaymentDetailEntities(orderPaymentDetailCount));

            IEnumerable<OrderPaymentDetailEntity> fraudRisks = CreateOrderPaymentDetailEntities(fraudRecords);

            ShopifyFraudDownloader testObject = new ShopifyFraudDownloader();

            testObject.Merge(order, fraudRisks);

            Assert.Equal(expectedFinalCount, order.OrderPaymentDetails.Select(opd => new { opd.Label, opd.Value }).Distinct().Count());
        }

        private static IEnumerable<OrderPaymentDetailEntity> CreateOrderPaymentDetailEntities(int count)
        {
            List<OrderPaymentDetailEntity> paymentDetails = new List<OrderPaymentDetailEntity>();

            for (int i = 0; i < count; i++)
            {
                paymentDetails.Add(
                    new OrderPaymentDetailEntity()
                    {
                        Label = $"Label-{i}",
                        Value = $"Value-{i}"
                    });
            }

            return paymentDetails;
        }

        /// <summary>
        /// Parse the fraud risks
        /// </summary>
        /// <param name="risks">Fraud risks text that should be parsed</param>
        private static IEnumerable<JToken> ParseFraudRisks(string risks)
        {
            if (risks.IsNullOrWhiteSpace())
            {
                return Enumerable.Empty<JToken>();
            }

            return JObject.Parse(risks)?
                .SelectToken("risks")
                .Where(x => x != null);
        }

        public void Dispose()
        {
            mock.Dispose();
        }
    }
}
