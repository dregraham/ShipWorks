using System;
using System.Linq;
using ShipWorks.Stores.Platforms.Shopify.DTOs;

namespace ShipWorks.Stores.Platforms.Shopify
{
    /// <summary>
    /// Shopify shipment has already uploaded tracking information Exception 
    /// </summary>
    public class ShopifyUnprocessableEntityException : ShopifyException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyUnprocessableEntityException()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyUnprocessableEntityException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ShopifyUnprocessableEntityException(Exception ex, ShopifyError errors)
            : base(ex.Message, ex)
        {
            Errors = errors;
        }

        /// <summary>
        /// Errors encountered while processing
        /// </summary>
        public ShopifyError Errors { get; }

        /// <summary>
        /// Is the error for an already uploaded order
        /// </summary>
        public bool IsAlreadyUploaded =>
            Errors?.Order.Any(y => y == "is already fulfilled") ?? true;

        /// <summary>
        /// Is the error for invalid locations
        /// </summary>
        public bool IsInvalidLocation =>
            Errors?.LineItems.Any(y => y == "must be stocked at the same location") ?? false;
    }
}
