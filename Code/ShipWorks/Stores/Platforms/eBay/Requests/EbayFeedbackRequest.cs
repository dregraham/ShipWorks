using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests
{
    /// <summary>
    /// An implementation of the IFeedbackRequest interface that is responsible for
    /// making requests to eBay to download feedabck.
    /// </summary>
    public class EbayFeedbackRequest : EbayRequest, IFeedbackRequest
    {
        // Seems that the API was only returning 25 entries per page in the response 
        // even if 100 entries per page was sent in the request
        private const int MaximumPageSize = 25;

        private GetFeedbackRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbayFeedbackRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbayFeedbackRequest(TokenData tokenData)
            : base(tokenData, "GetFeedback")
        {
            request = new GetFeedbackRequestType()
            {
                DetailLevel = new DetailLevelCodeType[] { DetailLevelCodeType.ReturnAll },

                Pagination = new PaginationType()
                {
                    
                    EntriesPerPageSpecified = true,
                    EntriesPerPage = MaximumPageSize,
                    PageNumber = 1,
                    PageNumberSpecified = true,
                }
            };
        }


        /// <summary>
        /// Gets the feedback download summary.
        /// </summary>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetFeedbackDownloadSummary()
        {
            // Return the minimal amount of data - just enough to extract the summary information
            request.Pagination.EntriesPerPage = 1;
            request.Pagination.PageNumber = 1;

            GetFeedbackResponseType response = SubmitRequest() as GetFeedbackResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain feedback from eBay.");
            }

            // Use the pagination information to create the download summary
            return new EbayDownloadSummary(response.PaginationResult.TotalNumberOfEntries, MaximumPageSize, DateTime.MinValue, DateTime.Now);
        }

        /// <summary>
        /// Gets the feedback details.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetFeedbackResponseType object.</returns>
        public GetFeedbackResponseType GetFeedbackDetails(int pageNumber)
        {
            request.Pagination.EntriesPerPage = MaximumPageSize;
            request.Pagination.PageNumber = pageNumber;

            GetFeedbackResponseType response = SubmitRequest() as GetFeedbackResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain feedback items from eBay.");
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
            return "GetFeedback";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>
        /// An AbstractRequestType object.
        /// </returns>
        public override WebServices.AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
