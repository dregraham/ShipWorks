using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using Xunit;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class DownloadOrdersRequestTest
    {
        private Credentials credentials;

        private Mocked.MockedNeweggRequest successfulRequest;
        private Mocked.MockedNeweggRequest failureRequest;
        private Mocked.MockedNeweggRequest zeroOrdersRequest;
        private Mocked.MockedNeweggRequest fortyTwoOrdersRequest;
        private Mocked.MockedNeweggRequest twoHundredOrdersRequest;

        public DownloadOrdersRequestTest()
        {
            string successfulResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Stores.Platforms.Newegg.Artifacts.DownloadedOrders.xml");
            string zeroOrdersResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Stores.Platforms.Newegg.Artifacts.ZeroOrderDownload.xml");
            string fortyTwoOrdersResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Stores.Platforms.Newegg.Artifacts.FortyTwoOrderDownload.xml");
            string twoHundredOrdersResponseXml = GetEmbeddedResourceXml("ShipWorks.Tests.Stores.Platforms.Newegg.Artifacts.MockedTwoHundredOrderDownload.xml");

            string errorResponseXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Errors xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <Error>
    <Code>CE010</Code>
    <Message>Invalid Date From.</Message>
  </Error>
</Errors>";

            successfulRequest = new Mocked.MockedNeweggRequest(successfulResponseXml);
            failureRequest = new Mocked.MockedNeweggRequest(errorResponseXml);
            zeroOrdersRequest = new Mocked.MockedNeweggRequest(zeroOrdersResponseXml);
            fortyTwoOrdersRequest = new Mocked.MockedNeweggRequest(fortyTwoOrdersResponseXml);
            twoHundredOrdersRequest = new Mocked.MockedNeweggRequest(twoHundredOrdersResponseXml);

            credentials = new Credentials("A09V", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
        }

        private string GetEmbeddedResourceXml(string embeddedResourceName)
        {
            string xml = string.Empty;
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(embeddedResourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    xml = reader.ReadToEnd();
                }
            }

            return xml;
        }

        [Fact]
        public void GetDownloadInfo_ThrowsInvalidOperationException_WhenErrorResponseReceived_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, failureRequest);
            Assert.Throws<NeweggException>(() => testObject.GetDownloadInfo(DateTime.UtcNow, DateTime.UtcNow, NeweggOrderType.All));
        }

        [Fact]
        public void GetDownloadInfo_ReturnsDownloadInfo_WhenSuccessfulResponseReceived_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(DateTime.UtcNow, DateTime.UtcNow, NeweggOrderType.All);

            Assert.NotNull(info);
            Assert.IsAssignableFrom<DownloadInfo>(info);
        }

        [Fact]
        public void GetDownloadInfo_ReturnsDownloadInfoWithTotalOrders_WhenSuccessfulResponseReceived_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(DateTime.UtcNow, DateTime.UtcNow, NeweggOrderType.All);

            // The number of orders in our DownloadedOrders.xml file
            Assert.Equal(42, info.TotalOrders);
        }

        [Fact]
        public void GetDownloadInfo_ReturnsDownloadInfoWithStartDate_WhenSuccessfulResponseReceived_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DateTime fromDate = DateTime.Parse("5/1/2012");
            DateTime toDate = DateTime.Parse("6/1/2012");

            DownloadInfo info = testObject.GetDownloadInfo(fromDate, toDate, NeweggOrderType.All);

            Assert.Equal(fromDate, info.StartDate);
        }

        [Fact]
        public void GetDownloadInfo_ReturnsDownloadInfoWithEndDate_WhenSuccessfulResponseReceived_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DateTime fromDate = DateTime.Parse("5/1/2012");
            DateTime toDate = DateTime.Parse("6/1/2012");

            DownloadInfo info = testObject.GetDownloadInfo(fromDate, toDate, NeweggOrderType.All);

            Assert.Equal(toDate, info.EndDate);
        }


        [Fact]
        public void GetDownloadInfo_PageCountIsZero_WhenZeroOrdersToDownload_Test()
        {
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, zeroOrdersRequest);
            DateTime fromDate = DateTime.Parse("5/1/2012");
            DateTime toDate = DateTime.Parse("6/1/2012");

            DownloadInfo info = testObject.GetDownloadInfo(fromDate, toDate, NeweggOrderType.All);

            Assert.Equal(0, info.PageCount);
        }

        [Fact]
        public void GetDownloadInfo_PageCountIsOne_WhenFortyTwoOrdersToDownload_Test()
        {
            // A test case for total orders not being evenly divided by max page size - page count
            // should be "rounded up" to the next integer value
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, fortyTwoOrdersRequest);
            DateTime fromDate = DateTime.Parse("5/1/2012");
            DateTime toDate = DateTime.Parse("6/1/2012");

            DownloadInfo info = testObject.GetDownloadInfo(fromDate, toDate, NeweggOrderType.All);

            Assert.Equal(1, info.PageCount);
        }

        [Fact]
        public void GetDownloadInfo_PageCountIsTwo_WhenTwoHundredOrdersToDownload_Test()
        {
            // A test case for total orders being evenly divided by the max page size
            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, twoHundredOrdersRequest);
            DateTime fromDate = DateTime.Parse("5/1/2012");
            DateTime toDate = DateTime.Parse("6/1/2012");

            DownloadInfo info = testObject.GetDownloadInfo(fromDate, toDate, NeweggOrderType.All);

            Assert.Equal(2, info.PageCount);
        }

        [Fact]
        public void GetDownloadInfo_FindsStartDate_FromListOfOrders_Test()
        {
            DateTime expectedStartDateInPST = TimeZoneInfo.ConvertTime(DateTime.Parse("1/5/2011 4:32:02 AM"), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            DateTime expectedStartDateInUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(expectedStartDateInPST, "Pacific Standard Time", "GMT Standard Time");

            List<Order> orders = new List<Order>
            {
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/13/2012 3:52:41 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("2/3/2012 8:37:07 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("1/2/2012 9:31:43 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("12/1/2012 4:29:19 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("4/1/2012 4:42:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/7/2012 6:01:43 PM") },
                new Order { OrderDateInPacificStandardTime = expectedStartDateInPST },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("6/12/2012 2:22:23 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
            };

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(expectedStartDateInUtc, info.StartDate);
        }

        [Fact]
        public void GetDownloadInfo_FindsEndDate_FromListOfOrders_Test()
        {
            DateTime expectedEndDateInPST = TimeZoneInfo.ConvertTime(DateTime.Parse("12/1/2012 4:29:19 PM"), TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
            DateTime expectedEndDateInUtc = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(expectedEndDateInPST, "Pacific Standard Time", "GMT Standard Time");

            List<Order> orders = new List<Order>
            {
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/13/2012 3:52:41 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("2/3/2012 8:37:07 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("1/2/2012 9:31:43 PM") },
                new Order { OrderDateInPacificStandardTime = expectedEndDateInPST },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("4/1/2012 4:42:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/7/2012 6:01:43 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("1/5/2011 4:32:02 AM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("6/12/2012 2:22:23 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
            };

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(expectedEndDateInUtc, info.EndDate);
        }

        [Fact]
        public void GetDownloadInfo_FindsTotalOrders_FromListOfOrders_Test()
        {
            List<Order> orders = new List<Order>
            {
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/13/2012 3:52:41 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("2/3/2012 8:37:07 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("1/2/2012 9:31:43 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("4/1/2012 4:42:01 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("9/7/2012 6:01:43 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("1/5/2011 4:32:02 AM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("6/12/2012 2:22:23 PM") },
                new Order { OrderDateInPacificStandardTime = DateTime.Parse("3/1/2012 4:32:01 PM") },
            };

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(orders.Count, info.TotalOrders);
        }

        [Fact]
        public void GetDownloadInfo_CalculatesPageCount_FromListOfOrders_WhenFortyTwoOrders_Test()
        {
            List<Order> orders = new List<Order>();
            for (int i = 0; i < 42; i++)
            {
                orders.Add(new Order());
            }

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(1, info.PageCount);
        }

        [Fact]
        public void GetDownloadInfo_CalculatesPageCount_FromListOfOrders_WhenZeroOrders_Test()
        {
            List<Order> orders = new List<Order>();

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(0, info.PageCount);
        }

        [Fact]
        public void GetDownloadInfo_StartDateIsNow_FromListOfOrders_WhenZeroOrders_Test()
        {
            DateTime utcNowForComparison = DateTime.UtcNow;
            List<Order> orders = new List<Order>();

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.True(utcNowForComparison <= info.StartDate);
        }

        [Fact]
        public void GetDownloadInfo_EndDateIsNow_FromListOfOrders_WhenZeroOrders_Test()
        {
            DateTime utcNowForComparison = DateTime.UtcNow;
            List<Order> orders = new List<Order>();

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.True(utcNowForComparison <= info.EndDate);
        }

        [Fact]
        public void GetDownloadInfo_CalculatesPageCount_FromListOfOrders_WhenTwoHundredOrders_Test()
        {
            List<Order> orders = new List<Order>();
            for (int i = 0; i < 200; i++)
            {
                orders.Add(new Order());
            }

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            DownloadInfo info = testObject.GetDownloadInfo(orders);

            Assert.Equal(2, info.PageCount);
        }

        [Fact]
        public void Download_RequestBodyContainsOrderNumberNodes_WhenDownloadingSpecificOrders_Test()
        {
            List<Order> ordersToDownload = new List<Order>
            {
                new Order { OrderNumber = 123456 },
                new Order { OrderNumber = 654321 }
            };

            DownloadOrdersRequest testObject = new DownloadOrdersRequest(credentials, successfulRequest);
            testObject.Download(ordersToDownload, 1);

            // Check that the XML contains the order numbers we provided
            XmlDocument requestBodyXml = new XmlDocument();
            requestBodyXml.LoadXml(successfulRequest.Body);
            XmlNodeList orderNumberNodes = requestBodyXml.SelectNodes("NeweggAPIRequest/RequestBody/RequestCriteria/OrderNumberList/OrderNumber");

            Assert.Equal(2, orderNumberNodes.Count);
            Assert.Equal("123456", orderNumberNodes[0].InnerText);
            Assert.Equal("654321", orderNumberNodes[1].InnerText);
        }
    }
}
