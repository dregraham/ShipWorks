using Xunit;
using ShipWorks.Stores.Platforms.Newegg;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;

namespace ShipWorks.Tests.Stores.Newegg
{
    public class CancellationRequestTest
    {
        private INeweggRequest successfulRequest;
        private INeweggRequest failedRequest;

        private Credentials credentials;

        private int orderNumberToCancel = 159243598;
        private string expectedCancelledSellerId = "ABCD";
        private string expectedCancelledOrderStatus = "Void";

        private Order orderToCancel;
        private CancelOrderRequest testObject;

        public CancellationRequestTest()
        {
            credentials = new Credentials(string.Empty, string.Empty, NeweggChannelType.US);
            orderToCancel = new Order { OrderNumber = orderNumberToCancel };

            string failureResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <Errors>
                      <Error>
                        <Code>SO002</Code>
                        <Message>Order Number should be an integer (ranging from 1 to 2147483647)</Message>
                      </Error>
                    </Errors>";

            string successResponse = string.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
                    <UpdateOrderStatusInfo>
                      <IsSuccess>true</IsSuccess>
                      <Result>
                        <OrderNumber>{0}</OrderNumber>
                        <SellerID>{1}</SellerID>
                        <OrderStatus>{2}</OrderStatus>
                      </Result>
                    </UpdateOrderStatusInfo>", orderNumberToCancel, expectedCancelledSellerId, expectedCancelledOrderStatus);


            failedRequest = new Mocked.MockedNeweggRequest(failureResponse);
            successfulRequest = new Mocked.MockedNeweggRequest(successResponse);
        }

        [Fact]
        public void Cancel_ThrowsNeweggException_WhenErrorResponseIsReceived_Test()
        {
            testObject = new CancelOrderRequest(credentials, failedRequest);
            Assert.Throws<NeweggException>(() => testObject.Cancel(orderToCancel, CancellationReason.OutOfStock));
        }

        [Fact]
        public void Cancel_ReturnsResultWithOrderNumber_WhenCancellingAnOrder_Test()
        {
            testObject = new CancelOrderRequest(credentials, successfulRequest);

            CancellationResult result = testObject.Cancel(orderToCancel, CancellationReason.OutOfStock);

            Assert.Equal(orderToCancel.OrderNumber, result.Detail.OrderNumber);
        }

        [Fact]
        public void Cancel_ReturnsResultWithVoidStatus_WhenCancellingAnOrder_Test()
        {
            testObject = new CancelOrderRequest(credentials, successfulRequest);

            CancellationResult result = testObject.Cancel(orderToCancel, CancellationReason.OutOfStock);

            Assert.Equal(expectedCancelledOrderStatus, result.Detail.OrderStatus);
        }

        [Fact]
        public void Cancel_ReturnsResultWithSellerId_WhenCancellingAnOrder_Test()
        {
            testObject = new CancelOrderRequest(credentials, successfulRequest);

            CancellationResult result = testObject.Cancel(orderToCancel, CancellationReason.OutOfStock);

            Assert.Equal(expectedCancelledSellerId, result.Detail.SellerId);
        }
        
        public void Cancel_ThrowsNeweggException_WhenCancellingAnInvoicedOrder_WithNeweggAPI_IntegrationTest()
        {
            // We're going to try to bounce the request off of the Newegg API, so setup 
            // the test object to use a "live" NeweggHttpRequest and an order setup in
            // our sandbox seller account, and use the sandbox seller account credentials
            Order sandboxedOrderToCancel = new Order { OrderNumber = 137956884 };
            Credentials credentials = new Credentials("A09V", "E09799F3-A8FD-46E0-989F-B8587A1817E0", NeweggChannelType.US);
            testObject = new CancelOrderRequest(credentials, new Mocked.NonLoggingNeweggRequest());

            // This should throw an exception since the order we're trying to cancel has
            // already been invoiced
            Assert.Throws<NeweggException>(() => testObject.Cancel(sandboxedOrderToCancel, CancellationReason.UnableToFulfillOrder));
        }
    }
}
