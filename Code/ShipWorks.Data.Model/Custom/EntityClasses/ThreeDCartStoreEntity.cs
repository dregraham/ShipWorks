using System;
using System.Text.RegularExpressions;
using Interapptive.Shared.Utility;

namespace ShipWorks.Data.Model.EntityClasses
{
    public partial class ThreeDCartStoreEntity
    {
        TimeZoneInfo storeTimeZone;

        /// <summary>
        /// The store's url domain
        /// </summary>
        public string StoreDomain
        {
            get
            {
                RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
                string domain = string.Empty;
                Regex urlDomainRegex = new Regex(@"^(?:\w+://)?([^/?]*)", regexOptions);

                Match match = urlDomainRegex.Match(StoreUrl);
                if (match.Success)
                {
                    domain = match.Groups[1].Value;
                }

                return domain;
            }
        }

        /// <summary>
        /// The TimeZoneInfo represented by the store's StoreTimeZoneInfoId
        /// If StoreTimeZoneInfoId is null, TimeZoneInfo.Local is returned.
        /// </summary>
        public TimeZoneInfo StoreTimeZone
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TimeZoneID))
                {
                    storeTimeZone = TimeZoneInfo.Local;
                }
                else if (   (storeTimeZone == null)
                         || (storeTimeZone != null && storeTimeZone.Id != TimeZoneID))
                {
                    storeTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneID);
                }

                return storeTimeZone;
            }
            set
            {
                storeTimeZone = value;
            }
        }

        /// <summary>
        /// Returns a date time, converted to the store's time zone
        /// Defaults to use UTC as the fromTimeZoneInfoId's time zone
        /// </summary>
        /// <returns>
        /// Returns the date time converted to the store's time zone if the store has a time zone specified,
        /// otherwise, the passed in date time is returned.
        /// </returns>
        public DateTime ConvertToStoreDateTime(DateTime fromTimeZoneInfoId)
        {
            return ConvertToStoreDateTime(fromTimeZoneInfoId, TimeZoneInfo.Utc.Id);
        }

        /// <summary>
        /// Returns a date time, converted to the store's timezone
        /// </summary>
        /// <param name="dateTimeToConvert">DateTime to convert to the store's time zone</param>
        /// <param name="fromTimeZoneInfoId">Time Zone Info ID to use as the dateTimeToConvert's time zone</param>
        /// <returns>
        /// Returns the date time converted to the store's time zone if the store has a time zone specified,
        /// otherwise, the passed in date time is returned.
        /// </returns>
        public DateTime ConvertToStoreDateTime(DateTime dateTimeToConvert, string fromTimeZoneInfoId)
        {
            DateTime convertedDateTime;

            // Default the time zone to the local time zone
            string toTimeZoneInfoId = TimeZoneInfo.Local.Id;

            // If the store has a time zone specified, set the to time zone to it
            if (!string.IsNullOrWhiteSpace(TimeZoneID))
            {
                toTimeZoneInfoId = TimeZoneID;
            }

            // If the from time zone is null/blank, use the local time zone
            if (string.IsNullOrWhiteSpace(fromTimeZoneInfoId))
            {
                fromTimeZoneInfoId = TimeZoneInfo.Local.Id;
            }

            convertedDateTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(dateTimeToConvert, fromTimeZoneInfoId, toTimeZoneInfoId);
            return convertedDateTime;
        }
    }
}
