using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Stores.Platforms.Ebay.Authorization;
using ShipWorks.Stores.Platforms.Ebay.WebServices;
using ShipWorks.Stores.Platforms.Ebay.Requests;

namespace ShipWorks.Stores.Platforms.Ebay.Requests.Download
{
    /// <summary>
    /// An implementation of the ISellingRequest interface that is responsible for
    /// making requests to eBay to obtain selling data.
    /// </summary>
    public class EbaySellingRequest : EbayRequest, ISellingRequest
    {
        // Limits imposed by eBay
        private const int MaximumPageSize = 25;

        private GetMyeBaySellingRequestType request;

        /// <summary>
        /// Initializes a new instance of the <see cref="EbaySellingRequest"/> class.
        /// </summary>
        /// <param name="tokenData">The token data.</param>
        public EbaySellingRequest(TokenData tokenData)
            : base(tokenData, "GetMyeBaySelling")
        {
            request = new GetMyeBaySellingRequestType()
            {
                SoldList = new ItemListCustomizationType()
                {
                    IncludeNotes = true,
                    IncludeNotesSpecified = true,
                    DurationInDaysSpecified = true,
                
                    Pagination = new PaginationType()
                    {
                        EntriesPerPage = MaximumPageSize,
                        EntriesPerPageSpecified = true,
                        PageNumber = 1,
                        PageNumberSpecified = true
                    }
                }
            };


        }

        /// <summary>
        /// Gets the maximum duration in days.
        /// </summary>
        public int MaximumDurationInDays
        {
            // eBay only permits going back 60 days
            get { return 60; }
        }


        /// <summary>
        /// Gets the sold items summary.
        /// </summary>
        /// <param name="durationInDays">The duration in days.</param>
        /// <returns>An EbayDownloadSummary object.</returns>
        public EbayDownloadSummary GetSoldItemsSummary(int durationInDays)
        {
            // Create a default download summary object with 0 transactons in the event that there are no items retrieved from eBay
            EbayDownloadSummary summary = new EbayDownloadSummary(0, MaximumPageSize, DateTime.Now.AddDays(durationInDays * -1), DateTime.Now);

            ValidateDuration(durationInDays);

            // Bring back minimal data (i.e. one item per page) because we just want
            // to get the pagination information
            request.SoldList.DurationInDays = durationInDays;
            request.SoldList.Pagination.PageNumber = 1;
            request.SoldList.Pagination.EntriesPerPage = 1;

            GetMyeBaySellingResponseType response = SubmitRequest() as GetMyeBaySellingResponseType;
            if (response == null)
            {
                throw new EbayException("Unable to obtain sold items from eBay.");
            }
                                    
            if (response.SoldList != null && response.SoldList.PaginationResult != null)
            {
                // There are items in the sold list so use the pagination information to create the download summary
                summary = new EbayDownloadSummary(response.SoldList.PaginationResult.TotalNumberOfEntries, MaximumPageSize, DateTime.Now.AddDays(durationInDays * -1), DateTime.Now);
            }

            return summary;
        }

        /// <summary>
        /// Gets the sold items.
        /// </summary>
        /// <param name="durationInDays">The duration in days.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An GetMyeBaySellingRequestType object.</returns>
        public GetMyeBaySellingResponseType GetSoldItems(int durationInDays, int pageNumber)
        {
            ValidateDuration(durationInDays);

            // The duration value is acceptable, so configure and submit the request
            request.SoldList.DurationInDays = durationInDays;
            request.SoldList.Pagination.PageNumber = pageNumber;
            GetMyeBaySellingResponseType response = SubmitRequest() as GetMyeBaySellingResponseType;

            if (response == null)
            {
                throw new EbayException("Unable to obtain sold items from eBay.");
            }

            return response;
        }

        /// <summary>
        /// Validations the duration.
        /// </summary>
        /// <param name="durationInDays">The duration in days.</param>
        private void ValidateDuration(int durationInDays)
        {
            if (durationInDays <= 0)
            {
                throw new EbayException("The duration must be greater than zero.");
            }

            if (durationInDays > MaximumDurationInDays)
            {
                throw new EbayException(string.Format("The duration exceeded the maximum allowable value of {0} days.", MaximumDurationInDays));
            }
        }

        /// <summary>
        /// Gets the name of the call as it is known to eBay. This value gets used
        /// as a query string parameter sent to eBay.
        /// </summary>
        /// <returns></returns>
        public override string GetEbayCallName()
        {
            return "GetMyeBaySelling";
        }

        /// <summary>
        /// Gets the specific eBay request (GetUserRequestType, GetOrdersRequestType, etc.) containing any parameter data necessary to make the request.
        /// </summary>
        /// <returns>An GetMyeBaySellingRequestType object.</returns>
        public override AbstractRequestType GetEbayRequest()
        {
            return request;
        }
    }
}
