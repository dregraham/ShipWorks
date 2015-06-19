using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.ItemRemoval
{
    /// <summary>
    /// An implementation of the IRemoveItemRequest interface that hits the Newegg API. 
    /// This will remove item(s) in specified order, but only supports order(s) that is 
    /// fulfilled by seller. If all items removed from an order, order status becomes void.
    /// </summary>
    public class RemoveItemRequest : IRemoveItemRequest
    {
        private const string RequestUrl = "{0}/ordermgmt/killitem/orders/{1}/?sellerid={2}";

        private Credentials credentials;
        private INeweggRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveItemRequest"/> class.
        /// This will use a NeweggHttpRequest object for issuing the requests.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public RemoveItemRequest(Credentials credentials)
            : this(credentials, new NeweggHttpRequest())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveItemRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="request">The request.</param>
        public RemoveItemRequest(Credentials credentials, INeweggRequest request)
        {
            this.credentials = credentials;
            this.request = request;
        }

        /// <summary>
        /// Removes the items from the given order.
        /// </summary>
        /// <param name="order">The order items should be removed from.</param>
        /// <param name="items">The items to be removed.</param>
        /// <returns>An ItemRemovalResult object.</returns>
        public ItemRemovalResult RemoveItems(Order order, IEnumerable<Item> items)
        {
            if (order == null)
            {
                throw new ArgumentNullException("The order parameter cannot not be null");
            }

            // API URL depends on which marketplace the seller selected 
            string marketplace = "";

            switch (credentials.Channel)
            {
                case NeweggChannelType.US:
                    break;
                case NeweggChannelType.Business:
                    marketplace = "/b2b";
                    break;
                case NeweggChannelType.Canada:
                    marketplace = "/can";
                    break;
                default:
                    break;
            }

            string formattedUrl = string.Format(RequestUrl, marketplace, order.OrderNumber, credentials.SellerId);

            RequestConfiguration requestConfig = new RequestConfiguration("Remove Items", formattedUrl) 
            { 
                Method = HttpVerb.Put, 
                Body = GetRequestBody(items) 
            };

            // The removal response data should contain the XML describing an ItemRemovalResult
            string responseData = this.request.SubmitRequest(credentials, requestConfig);
            NeweggResponse removalResponse = new NeweggResponse(responseData, new ItemRemovalResponseSerializer());

            if (removalResponse.ResponseErrors.Count() > 0)
            {
                string errorMessage = string.Format("An error was encountered while removing items from order number {0}{1}.", order.OrderNumber, System.Environment.NewLine);
                foreach (Error error in removalResponse.ResponseErrors)
                {
                    errorMessage += string.Format("{0} (error code {1})", error.Description, error.ErrorCode) + System.Environment.NewLine;
                }

                throw new NeweggException(errorMessage, removalResponse);
            }

            // There weren't any response errors, so the result in the response is an ItemRemovalResult
            ItemRemovalResult removalResult = removalResponse.Result as ItemRemovalResult;

            // There's one last check we should make to ensure the items were removed successfully (another
            // case where this API method is a little odd and deviates from the other methods)
            if (!removalResult.IsSuccessful)
            {
                // The request was not successful, so check the memo field for a detailed error description
                string errorMessage = "An error was encountered while removing items from order number {0}." + System.Environment.NewLine;
                errorMessage += removalResult.Memo;

                throw new NeweggException(errorMessage, removalResponse);
            }

            return removalResult;
        }

        /// <summary>
        /// A helper method to build the body of the request based on the items provided.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The string to send to Newegg to process the request.</returns>
        private static string GetRequestBody(IEnumerable<Item> items)
        {
            StringBuilder requestBody = new StringBuilder();
            requestBody.Append(@"
                    <NeweggAPIRequest>
                      <OperationType>KillItemRequest</OperationType>
                      <RequestBody>
                        <KillItem>
                          <Order>
                            <ItemList>");

            foreach(Item item in items)
            {
                requestBody.AppendFormat("<Item><SellerPartNumber>{0}</SellerPartNumber></Item>", item.SellerPartNumber);                              
            }

            requestBody.Append(@"
                            </ItemList>
                          </Order>
                        </KillItem>
                      </RequestBody>
                    </NeweggAPIRequest>");

            return requestBody.ToString();
        }
    }
}
