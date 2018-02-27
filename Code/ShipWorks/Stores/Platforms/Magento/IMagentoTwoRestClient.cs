﻿using System;
using System.Collections.Generic;
using ShipWorks.Stores.Platforms.Magento.DTO.MagnetoTwoRestOrder;

namespace ShipWorks.Stores.Platforms.Magento
{
    /// <summary>
    /// Represents the Magento Two REST Web Client
    /// </summary>
    public interface IMagentoTwoRestClient
    {
        /// <summary>
        /// Gets Orders from the store using the start date
        /// </summary>
        OrdersResponse GetOrders(DateTime? start, int currentPage);

        /// <summary>
        /// Gets a single Magento order with detailed information (attributes)
        /// </summary>
        Order GetOrder(long magentoOrderId);

        /// <summary>
        /// Gets a token for the given username/password
        /// </summary>
        string GetToken();

        /// <summary>
        /// Uploads the shipment details.
        /// </summary>
        void UploadShipmentDetails(string shipment, string invoice, long magentoOrderId);

        /// <summary>
        /// Uploads comments only
        /// </summary>
        void UploadComments(string comments, long magentoOrderID, bool commentsOnly);

        /// <summary>
        /// Place a hold on a Magento order
        /// </summary>
        void HoldOrder(long magentoOrderID);

        /// <summary>
        /// Take hold off of a Magento order
        /// </summary>
        void UnholdOrder(long magentoOrderID);

        /// <summary>
        /// Cancels a Magento order
        /// </summary>
        void CancelOrder(long magentoOrderID);

        /// <summary>
        /// Gets the specified item
        /// </summary>
        /// <returns></returns>
        Item GetItem(long itemId);

        /// <summary>
        /// Gets the product for the given sku
        /// </summary>
        /// <param name="sku"></param>
        /// <returns>
        /// Product is similar to item except it contains product details
        /// and is not order specific details like option names and image urls
        /// </returns>
        Product GetProductBySku(string sku);
        
        /// <summary>
        /// Gets the product for the given Id
        /// </summary>
        /// <returns>
        /// Product is similar to item except it contains product details
        /// and is not order specific details like option names and image urls
        /// </returns>
        Product GetProductById(int productId);

        /// <summary>
        /// Get bundle production options by sku
        /// </summary>
        IEnumerable<ProductOptionDetail> GetBundleProductOptionsBySku(string sku);
    }
}