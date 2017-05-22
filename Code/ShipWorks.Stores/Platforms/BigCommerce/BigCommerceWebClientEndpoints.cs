using System;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.BigCommerce
{
    /// <summary>
    /// Api endpoints for communicating with BigCommerce
    /// </summary>
    public static class BigCommerceWebClientEndpoints
    {
        /// <summary>
        /// Returns the resource path for accessing a specific order's shipments.  Also used for creating a new shipment.
        /// </summary>
        /// <param name="orderID"></param>
        public static string GetUploadShipmentResource(long orderID)
        {
            return string.Format("orders/{0}/shipments", orderID);
        }

        /// <summary>
        /// Returns the resource path for getting the number of orders
        /// </summary>
        public static string GetOrderCountResource()
        {
            return "orders/count";
        }

        /// <summary>
        /// Returns the resource path for accessing a specific order
        /// </summary>
        /// <param name="orderId"></param>
        public static string GetOrderResource(long orderId)
        {
            return string.Format("orders/{0}", orderId);
        }
        
        /// <summary>
        /// Returns the resource path for downloading orders
        /// </summary>
        public static string GetOrdersResource()
        {
            return "orders";
        }

        /// <summary>
        /// Returns the resource path for downloading order statuses
        /// </summary>
        public static string GetOrderStatusesPath()
        {
            return "orderstatuses";
        }

        /// <summary>
        /// Returns the resource path for accessing product images
        /// </summary>
        /// <param name="productID"></param>
        public static string GetProductImagesResource(long productID)
        {
            return string.Format("products/{0}/images", productID);
        }

        /// <summary>
        /// Returns the resource path for accessing order products
        /// </summary>
        public static string GetOrderProducts(long orderID)
        {
            return string.Format("orders/{0}/products", orderID);
        }

        /// <summary>
        /// Returns the image url for a product
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <param name="imagePath"></param>
        public static string GetProductImageUrl(string apiUrl, string imagePath)
        {
            return string.Format("{0}/product_images/{1}", apiUrl.Replace("/api/v2/", string.Empty), imagePath);
        }
    }
}
