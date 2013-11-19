using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Tokens;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Stores.Platforms.Ebay
{
    /// <summary>
    /// Client that wraps all connectivity to the eBay API
    /// </summary>
    public class EbayWebClient
    {
        EbayToken token;

        /// <summary>
        /// Constructor
        /// </summary>
        public EbayWebClient(EbayToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }

            if (string.IsNullOrWhiteSpace(token.Token))
            {
                throw new ArgumentException("token cannot be blank", "token");
            }

            this.token = token;
        }

        /// <summary>
        /// Get eBay official time in UTC
        /// </summary>
        public DateTime GetOfficialTime()
        {
            EbayOfficialTimeRequest request = new EbayOfficialTimeRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get the user informatiopn for the user that is represented by the token
        /// </summary>
        public UserType GetUser()
        {
            EbayGetUserRequest request = new EbayGetUserRequest(token);

            return request.Execute();
        }

        /// <summary>
        /// Get a page of orders from eBay
        /// </summary>
        public GetOrdersResponseType GetOrders(DateTime rangeStart, DateTime rangeEnd, int page)
        {
            EbayGetOrdersRequest request = new EbayGetOrdersRequest(token, rangeStart, rangeEnd, page);

            return request.Execute();
        }

        /// <summary>
        /// Get full item details for the given item listing ID
        /// </summary>
        public ItemType GetItem(string itemID)
        {
            EbayGetItemRequest request = new EbayGetItemRequest(token, itemID);

            return request.Execute();
        }

        /// <summary>
        /// Get feedback that has been left by or for this user
        /// </summary>
        public GetFeedbackResponseType GetFeedback(int page)
        {
            EbayGetFeedbackRequest request = new EbayGetFeedbackRequest(token, page);

            return request.Execute();
        }
    }
}
