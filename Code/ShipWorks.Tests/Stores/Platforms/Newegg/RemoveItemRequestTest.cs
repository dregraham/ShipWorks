﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using Xunit;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class RemoveItemRequestTest
    {
        private INeweggRequest errorRequest;
        private INeweggRequest successfulRequest;
        private INeweggRequest failedRequest;

        private Credentials credentials;

        private int orderNumberToRemoveFrom = 159243598;
        private string sellerId = "ABCD";
        private string sellerPartNumberToRemove = "ASDF0987";

        private Order orderToRemoveItemsFrom;
        private Item itemToRemove;

        private RemoveItemRequest testObject;

        public RemoveItemRequestTest()
        {
            credentials = new Credentials(sellerId, string.Empty, NeweggChannelType.US);
            orderToRemoveItemsFrom = new Order { OrderNumber = orderNumberToRemoveFrom };
            itemToRemove = new Item { SellerPartNumber = sellerPartNumberToRemove };

            string errorResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <Errors>
                      <Error>
                        <Code>SO008</Code>
                        <Message>This order has already been voided</Message>
                      </Error>
                    </Errors>";

            string successResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <NeweggAPIResponse>
                          <IsSuccess>true</IsSuccess>
                          <OperationType>KillItemResponse</OperationType>
                          <SellerID>{0}</SellerID>
                          <Memo />
                          <ResponseBody>
                            <RequestDate>2012-02-22 16:42:10</RequestDate>
                            <Orders>
                              <OrderNumber>{1}</OrderNumber>
                              <Result>
                                <ItemList>
                                  <Item>
                                    <SellerPartNumber>{2}</SellerPartNumber>
                                  </Item>
                                </ItemList>
                              </Result>
                            </Orders>
                          </ResponseBody>
                          <ResponseDate>2012-02-22 16:42:10</ResponseDate>
                        </NeweggAPIResponse>", sellerId, orderNumberToRemoveFrom, sellerPartNumberToRemove);

            string failureResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <NeweggAPIResponse>
                          <IsSuccess>false</IsSuccess>
                          <OperationType>KillItemResponse</OperationType>
                          <SellerID>{0}</SellerID>
                          <Memo>Simulating a failure through the memo field</Memo>
                          <ResponseBody>
                            <RequestDate>2012-02-22 16:42:10</RequestDate>
                            <Orders />
                          </ResponseBody>
                          <ResponseDate>2012-02-22 16:42:10</ResponseDate>
                        </NeweggAPIResponse>", sellerId);

            errorRequest = new Mocked.MockedNeweggRequest(errorResponse);
            successfulRequest = new Mocked.MockedNeweggRequest(successResponse);
            failedRequest = new Mocked.MockedNeweggRequest(failureResponse);
        }

        [Fact]
        public async Task RemoveItems_ThrowsNeweggException_WhenErrorResponseIsReceived()
        {
            testObject = new RemoveItemRequest(credentials, errorRequest);
            await Assert.ThrowsAsync<NeweggException>(() => testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove }));
        }

        [Fact]
        public async Task RemoveItems_ThrowsNeweggException_WhenIsSuccessfulIsFalse()
        {
            testObject = new RemoveItemRequest(credentials, failedRequest);
            await Assert.ThrowsAsync<NeweggException>(() => testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove }));
        }

        [Fact]
        public async Task RemoveItems_ReturnsItemRemovalResult_WithItemsRemoved_WhenIsSuccessfulIsTrue()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);
            ItemRemovalResult result = await testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            Assert.Equal(1, result.Details.Order.ItemResult.ItemsRemoved.Count);
            Assert.Equal(sellerPartNumberToRemove, result.Details.Order.ItemResult.ItemsRemoved[0].SellerPartNumber);
        }

        [Fact]
        public async Task RemoveItems_ReturnsItemRemovalResult_WithOrderNumber_WhenIsSuccessfulIsTrue()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);
            ItemRemovalResult result = await testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            Assert.Equal(orderNumberToRemoveFrom, result.Details.Order.OrderNumber);
        }

        [Fact]
        public void RemoveItems_UsesPutRequestMethod()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);

            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request
            Assert.Equal(HttpVerb.Put, ((Mocked.MockedNeweggRequest) successfulRequest).Method);
        }

        [Fact]
        public void RemoveItems_FormatsUrlWithOrderNumberAndSellerId()
        {
            string expectedUrl = string.Format("https://api.newegg.com/marketplace/ordermgmt/killitem/orders/{0}/?sellerid={1}", orderNumberToRemoveFrom, sellerId);
            testObject = new RemoveItemRequest(credentials, successfulRequest);

            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request
            Assert.Equal(expectedUrl, ((Mocked.MockedNeweggRequest) successfulRequest).Url);
        }

        [Fact]
        public void RemoveItems_BuildsRequestBody_WithSellerPartNumbersOfItemsToRemove()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);

            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            XDocument requestDocument = XDocument.Parse(((Mocked.MockedNeweggRequest) successfulRequest).Body);
            var request = requestDocument.Descendants("NeweggAPIRequest").First();

            Assert.Equal("KillItemRequest", request.Element("OperationType").Value);
            Assert.Equal(sellerPartNumberToRemove, request.Descendants("SellerPartNumber").Single().Value);
        }
    }
}
