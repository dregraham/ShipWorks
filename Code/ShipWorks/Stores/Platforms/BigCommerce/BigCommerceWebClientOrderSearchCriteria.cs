using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.BigCommerce.Enums;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Helper class with properties for criteria used when searching the BigCommerce api for orders
    /// </summary>
    public class BigCommerceWebClientOrderSearchCriteria
    {
        /// <summary>
        /// Constructor and sets the searach criteria, converting paramaters as needed for the api
        /// </summary>
        /// <param name="orderDateSearchType">The date search type to perform, Created by date or modified date</param>
        /// <param name="lastModifiedFromDateTimeUtc">The UTC date for the start of the order modified date range</param>
        /// <param name="lastModifiedToDateTimeUtc">The UTC date for the end of the order modified date range</param>
        /// <param name="lastCreatedFromDateTimeUtc">The UTC date for the start of the order create date range</param>
        /// <param name="lastCreatedToDateTimeUtc">The UTC date for the end of the order create date range</param>
        public BigCommerceWebClientOrderSearchCriteria(BigCommerceWebClientOrderDateSearchType orderDateSearchType, DateTime lastModifiedFromDateTimeUtc, DateTime lastModifiedToDateTimeUtc,
            DateTime lastCreatedFromDateTimeUtc, DateTime lastCreatedToDateTimeUtc)
        {
            OrderDateSearchType = orderDateSearchType;
            LastModifiedFromDate = lastModifiedFromDateTimeUtc;
            LastModifiedToDate = lastModifiedToDateTimeUtc;
            LastCreatedFromDate = lastCreatedFromDateTimeUtc;
            LastCreatedToDate = lastCreatedToDateTimeUtc;
        }

        /// <summary>
        /// The type of order date search to perform
        /// </summary>
        public BigCommerceWebClientOrderDateSearchType OrderDateSearchType
        {
            get;
            set;
        }

        /// <summary>
        /// The converted start date of the order modified date range
        /// </summary>
        public DateTime LastModifiedFromDate
        {
            get;
            set;
        }

        /// <summary>
        /// The converted end date of the order modified date range
        /// </summary>
        public DateTime LastModifiedToDate
        {
            get;
            set;
        }

        /// <summary>
        /// The converted start date of the created date range
        /// </summary>
        public DateTime LastCreatedFromDate
        {
            get;
            set;
        }

        /// <summary>
        /// The converted end date of the created date range
        /// </summary>
        public DateTime LastCreatedToDate
        {
            get; 
            set;
        }

        /// <summary>
        /// The maximum number of results to return per page
        /// </summary>
        public int PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// The page of results to return
        /// </summary>
        public int Page
        {
            get;
            set;
        }
    }
}
