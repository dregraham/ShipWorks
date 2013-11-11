using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;
using ShipWorks.Stores.Platforms.Ebay.Tokens;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the ICombinedPaymentRequest interface that is responsible for
    /// making requests to eBay to download combined payment data.
    /// </summary>
    public class EbayGetOrdersRequest : EbayRequest<GetOrdersResponseType, GetOrdersRequestType, GetOrdersResponseType>
    {
        GetOrdersRequestType request; 

        // Max is 100
        const int PageSize = 50;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayGetOrdersRequest"/> class.
        /// </summary>
        public EbayGetOrdersRequest(EbayToken token, DateTime rangeStart, DateTime rangeEnd, int page)
            : base(token, "GetOrders")
        {
            request = new GetOrdersRequestType();
            request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };

            request.ModTimeFrom = rangeStart;
            request.ModTimeFromSpecified = true;

            request.ModTimeTo = rangeEnd;
            request.ModTimeToSpecified = true;

            request.OrderRole = TradingRoleCodeType.Seller;
            request.OrderRoleSpecified = true;

            request.OrderStatus = OrderStatusCodeType.All;
            request.OrderStatusSpecified = true;

            request.Pagination = new PaginationType
                {
                    EntriesPerPage = PageSize,
                    EntriesPerPageSpecified = true,

                    PageNumber = page,
                    PageNumberSpecified = true,
                };

            request.SortingOrder = SortOrderCodeType.Ascending;
            request.SortingOrderSpecified = true;
        }


        /// <summary>
        /// Gets the page of orders configured by the request
        /// </summary>
        public override GetOrdersResponseType Execute()
        {
            return SubmitRequest();
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
