using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the IItemTransactionRequest interface that is responsible for
    /// making requests to eBay to obtain details of a specific item/transaction.
    /// </summary>
    public class EbayItemTransactionRequest : EbayRequest, IItemTransactionRequest
    {
        private GetItemTransactionsRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayItemTransactionRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayItemTransactionRequest(TokenData tokenData)
            : base(tokenData, "GetItemTransactions")
        {
            request = new GetItemTransactionsRequestType();
        }

        /// <summary>
        /// Gets the transaction detail for a given item and transaction. (The eBay API does
        /// not have a way to get a transaction from a transaction ID.)
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <param name="transactionId">The transaction ID.</param>
        /// <returns>
        /// A GetItemTransactionsResponseType object.
        /// </returns>
        public GetItemTransactionsResponseType GetTransactionDetail(string itemId, string transactionId)
        {
            request.ItemID = itemId;
            request.TransactionID = transactionId;

            GetItemTransactionsResponseType response = SubmitRequest() as GetItemTransactionsResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain item transaction data from eBay.");
            }

            return response;
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "GetItemTransactions";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An GetItemTransactionsRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
