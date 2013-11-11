using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the IItemRequest interface that is responsible for
    /// making requests to eBay to download order item data.
    /// </summary>
    public class EbayItemRequest : EbayRequest, IItemRequest
    {
        private GetItemRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayItemRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayItemRequest(TokenData tokenData)
            : base(tokenData, "GetItem")
        {
            request = new GetItemRequestType()
            {
                DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ItemReturnAttributes }
            };
        }

        /// <summary>
        /// Gets the item details.
        /// </summary>
        /// <param name="itemId">The item ID.</param>
        /// <returns>A GetItemResponseType object.</returns>
        public GetItemResponseType GetItemDetails(string itemId)
        {
            request.ItemID = itemId;

            GetItemResponseType response = SubmitRequest() as GetItemResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain item details from eBay.");
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
            return "GetItem";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>A GetItemRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
