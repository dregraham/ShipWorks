using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Interapptive.Shared;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.ThreeDCart.Enums;

namespace ShipWorks.Stores.Platforms.ThreeDCart
{
    /// <summary>
    /// Helper class with properties for criteria used when searching the 3D Cart api for orders
    /// </summary>
    public class ThreeDCartWebClientOrderSearchCriteria
    {
        readonly ThreeDCartStoreEntity store;

        /// <summary>
        /// Constructor and sets the searach criteria, converting paramaters as needed for the api
        /// </summary>
        /// <param name="orderDateSearchType">The type of search to do, by modified date or created date </param>
        /// <param name="lastModifiedFromDateTimeUtc">The UTC date for the start of the order modified date range</param>
        /// <param name="lastModifiedToDateTimeUtc">The UTC date for the end of the order modified date range</param>
        /// <param name="lastCreatedFromDateTimeUtc">The UTC date for the start of the order create date range</param>
        /// <param name="lastCreatedToDateTimeUtc">The UTC date for the end of the order create date range</param>
        /// <param name="threeDCartStore">The 3D Cart store</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="page">The page of results to return.</param>
        [NDependIgnoreTooManyParams]
        public ThreeDCartWebClientOrderSearchCriteria(ThreeDCartWebClientOrderDateSearchType orderDateSearchType, 
            DateTime lastModifiedFromDateTimeUtc, DateTime lastModifiedToDateTimeUtc,
            DateTime lastCreatedFromDateTimeUtc, DateTime lastCreatedToDateTimeUtc, 
            ThreeDCartStoreEntity threeDCartStore, int pageSize, int page)
        {
            store = threeDCartStore; 

            // Set the search type
            OrderDateSearchType = orderDateSearchType;

            // Convert to store time zone so that threeDCart searches correctly
            LastModifiedFromDate = store.ConvertToStoreDateTime(lastModifiedFromDateTimeUtc);
            //LastModifiedToDate = lastModifiedToDateTimeUtc;
            PageSize = pageSize;
            Page = page;

            // 3D Cart currently has a bug where the last modified date is in eastern time zone on order creation, but is correct on subsequent updates to the order
            // So find the eastern time and if it is older than the passed in value, use it
            DateTime easternTimeZoneDateTimeUtc = TimeZoneInfo.ConvertTime(lastModifiedToDateTimeUtc, TimeZoneInfo.Utc, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToUniversalTime();
            if (easternTimeZoneDateTimeUtc > lastModifiedToDateTimeUtc)
            {
                lastModifiedToDateTimeUtc = easternTimeZoneDateTimeUtc;
            }

            // Convert to the store's time zone
            LastModifiedToDate = store.ConvertToStoreDateTime(lastModifiedToDateTimeUtc, TimeZoneInfo.Utc.Id);

            // Convert the created to/from dates to the store's time zone
            LastCreatedFromDate = store.ConvertToStoreDateTime(lastCreatedFromDateTimeUtc, TimeZoneInfo.Utc.Id);
            LastCreatedToDate = store.ConvertToStoreDateTime(lastCreatedToDateTimeUtc, TimeZoneInfo.Utc.Id);
        }

        /// <summary>
        /// The type of order date search to perform
        /// </summary>
        public ThreeDCartWebClientOrderDateSearchType OrderDateSearchType
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
            private set;
        }

        /// <summary>
        /// The converted end date of the order modified date range
        /// </summary>
        public DateTime LastModifiedToDate
        {
            get;
            private set;
        }

        /// <summary>
        /// The converted start date of the created date range
        /// </summary>
        public DateTime LastCreatedFromDate
        {
            get;
            private set;
        }

        /// <summary>
        /// The converted end date of the created date range
        /// </summary>
        public DateTime LastCreatedToDate
        {
            get;
            private set;
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
