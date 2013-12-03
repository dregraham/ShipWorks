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
    /// Implements the eBay GetFeedback API
    /// </summary>
    public class EbayGetFeedbackRequest : EbayRequest<GetFeedbackResponseType, GetFeedbackRequestType, GetFeedbackResponseType>
    {
        private const int pageSize = 25;

        // The request we are going to send
        GetFeedbackRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayFeedbackRequest"/> class.
        /// </summary>
        public EbayGetFeedbackRequest(EbayToken token, FeedbackTypeCodeType feedbackType, int page)
            : base(token, "GetFeedback")
        {
            request = new GetFeedbackRequestType();
            request.DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll };
            request.FeedbackType = feedbackType;
            request.FeedbackTypeSpecified = true;

            request.Pagination = new PaginationType
            {
                EntriesPerPage = page,
                EntriesPerPageSpecified = true,

                PageNumber = page,
                PageNumberSpecified = true,
            };
        }

        /// <summary>
        /// Gets the item details
        /// </summary>
        public override GetFeedbackResponseType Execute()
        {
            return SubmitRequest();
        }

        /// <summary>
        /// Create the request to be used for submission
        /// </summary>
        protected override AbstractRequestType CreateRequest()
        {
            return request;
        }
    }
}
