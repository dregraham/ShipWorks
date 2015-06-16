using System;
using System.Linq;
using System.Net;
using Interapptive.Shared.Net;
using ShipWorks.Stores.Platforms.Newegg.Net.Errors.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation.Response;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.Enums;

namespace ShipWorks.Stores.Platforms.Newegg.Net.Orders.Cancellation
{
    /// <summary>
    /// An implementation of the ICancelOrderRequest that hits the Newegg API.
    /// </summary>
    public class CancelOrderRequest : ICancelOrderRequest
    {
        private const string RequestUrl = "{0}/ordermgmt/orderstatus/orders/{1}?sellerid={2}";
        const int CancelledAction = 1;

        private Credentials credentials;
        private INeweggRequest request;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelOrderRequest"/> class. This
        /// constructor will use a NeweggHttpRequest object for all outbound requests.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        public CancelOrderRequest(Credentials credentials)
            : this (credentials, new NeweggHttpRequest())
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelOrderRequest"/> class.
        /// </summary>
        /// <param name="credentials">The credentials.</param>
        /// <param name="request">The request to be used for outbound requests.</param>
        public CancelOrderRequest(Credentials credentials, INeweggRequest request)
        {
            this.credentials = credentials;
            this.request = request;
        }

        /// <summary>
        /// Cancels a Newegg order.
        /// </summary>
        /// <param name="neweggOrder">The newegg order to be cancelled.</param>
        /// <param name="reason">The reason for cancelling the order.</param>
        /// <returns>A CancellationResult object.</returns>
        public CancellationResult Cancel(Order neweggOrder, CancellationReason reason)
        {
            // API URL depends on which marketplace the seller selected 
            string marketplace = (credentials.Channel == NeweggChannelType.Business) ? "/b2b" : "";

            // Format our request URL with the value of the seller ID and configure the request
            string formattedUrl = string.Format(RequestUrl, marketplace, neweggOrder.OrderNumber, credentials.SellerId);
            RequestConfiguration requestConfig = new RequestConfiguration("Cancelling order", formattedUrl)
            { 
                Method = HttpVerb.Put, 
                Body = GetRequestBody(reason) 
            };

            string responseData = this.request.SubmitRequest(credentials, requestConfig);

            // The cancellation data should contain the XML describing a CancellationResult
            NeweggResponse cancelResponse = new NeweggResponse(responseData, new CancelResponseSerializer());
            
            if (cancelResponse.ResponseErrors.Count() > 0)
            {
                string errorMessage = string.Format("An error was encountered while cancelling order number {0}{1}.", neweggOrder.OrderNumber, System.Environment.NewLine);
                foreach (Error error in cancelResponse.ResponseErrors)
                {
                    errorMessage += string.Format("{0} (error code {1})", error.Description, error.ErrorCode) + System.Environment.NewLine;
                }

                throw new NeweggException(errorMessage, cancelResponse);
            }

            // There weren't any errors, so the result in the response is a CancellationResult
            return cancelResponse.Result as CancellationResult;
        }

        private static string GetRequestBody(CancellationReason reason)
        {
            string requestBody = string.Format(
                @"<UpdateOrderStatus> 
                    <Action>{0}</Action> 
                    <Value>{1}</Value> 
                </UpdateOrderStatus>", CancelledAction, (int)reason);

            return requestBody;
        }
    }
}
