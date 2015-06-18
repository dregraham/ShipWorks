using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Tests.Stores.Newegg
{
    [TestClass]
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

        [TestInitialize]
        public void Initialize()
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

        [TestMethod]
        [ExpectedException(typeof(NeweggException))]
        public void RemoveItems_ThrowsNeweggException_WhenErrorResponseIsReceived_Test()
        {
            testObject = new RemoveItemRequest(credentials, errorRequest);
            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });
        }

        [TestMethod]
        [ExpectedException(typeof(NeweggException))]
        public void RemoveItems_ThrowsNeweggException_WhenIsSuccessfulIsFalse_Test()
        {
            testObject = new RemoveItemRequest(credentials, failedRequest);
            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });
        }

        [TestMethod]
        public void RemoveItems_ReturnsItemRemovalResult_WithItemsRemoved_WhenIsSuccessfulIsTrue_Test()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);
            ItemRemovalResult result = testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            Assert.AreEqual(1, result.Details.Order.ItemResult.ItemsRemoved.Count);
            Assert.AreEqual(sellerPartNumberToRemove, result.Details.Order.ItemResult.ItemsRemoved[0].SellerPartNumber);
        }

        [TestMethod]
        public void RemoveItems_ReturnsItemRemovalResult_WithOrderNumber_WhenIsSuccessfulIsTrue_Test()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);
            ItemRemovalResult result = testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            Assert.AreEqual(orderNumberToRemoveFrom, result.Details.Order.OrderNumber);
        }

        [TestMethod]
        public void RemoveItems_UsesPutRequestMethod_Test()
        {
            testObject = new RemoveItemRequest(credentials, successfulRequest);

            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            Assert.AreEqual(HttpVerb.Put, ((Mocked.MockedNeweggRequest)successfulRequest).Method);
        }

        [TestMethod]
        public void RemoveItems_FormatsUrlWithOrderNumberAndSellerId_Test()
        {
            string expectedUrl = string.Format("https://api.newegg.com/marketplace/ordermgmt/killitem/orders/{0}/?sellerid={1}", orderNumberToRemoveFrom, sellerId);
            testObject = new RemoveItemRequest(credentials, successfulRequest);

            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            Assert.AreEqual(expectedUrl, ((Mocked.MockedNeweggRequest)successfulRequest).Url);
        }

        [TestMethod]
        public void RemoveItems_BuildsRequestBody_WithSellerPartNumbersOfItemsToRemove_Test()
        {
            // Note: this is brittle as it requires the string to be in the exact same
            // format as that in the implementation. Look into incorporating XmlDiff 
            // to compare the two XML documents
            testObject = new RemoveItemRequest(credentials, successfulRequest);
            string expectedRequestBody = string.Format(@"<NeweggAPIRequest>
                      <OperationType>KillItemRequest</OperationType>
                      <RequestBody>
                        <KillItem>
                          <Order>
                            <ItemList><Item><SellerPartNumber>{0}</SellerPartNumber></Item>
                            </ItemList>
                          </Order>
                        </KillItem>
                      </RequestBody>
                    </NeweggAPIRequest>", sellerPartNumberToRemove);


            testObject.RemoveItems(orderToRemoveItemsFrom, new List<Item> { itemToRemove });

            // Since we configured our request with a mocked Newegg request we can inspect
            // the data/configuration of the request 
            Assert.AreEqual(0, string.Compare(expectedRequestBody, ((Mocked.MockedNeweggRequest)successfulRequest).Body.Trim()));
        }
    }
}
