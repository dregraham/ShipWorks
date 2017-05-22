using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Constants for accessing BigCommerce API
    /// </summary>
    public static class BigCommerceConstants
    {
        // 50 is the max that BigCommerce allows for most batch queries
        public const int OrdersPageSize = 50;

        // 250 is the max that BigCommerce allows for retrieving order products
        public const int MaxPageSize = 250;

        // .Net doesn't have 509 as a supported Http Status Code, so needed our own constant
        public const int MaxRequestsPerHourLimitReachedStatusCode = 509;

        // Orders can be marked deleted on the website, and this comes through as a flag and not an actual status
        // So to support it, we will treat it as an online status.  This is it's name and code.
        public const string OnlineStatusDeletedName = "Deleted";
        public const int OnlineStatusDeletedCode = -1;

        // Constant that is an invalid BigCommerce order address id.  Will use this to check if an order has
        // a shipping address.
        public const long InvalidOrderAddressID = -1;

        // As per BigCommerce, the order statuses are not customizable, so adding this constant so that we may
        // allow this status update for digital orders (other statuses will not be allowed)
        // The order must be in completed status for the customer to be able to download, so we don't want to 
        // change the status to anything but Completed.
        public const int OrderStatusCompleted = 10;
    }
}
