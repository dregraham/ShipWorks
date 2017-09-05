using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.Newegg.Enums;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Download;
using ShipWorks.Stores.Platforms.Newegg.Net.Orders.Response;
using ShipWorks.Stores.Platforms.Newegg.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Newegg
{
    /// <summary>
    /// Class that acts as a facade for funneling all requests to the Newegg API.
    /// </summary>
    public interface INeweggWebClient
    {
        /// <summary>
        /// Checks whether the credentials are valid by making a request to the Newegg API.
        /// </summary>
        /// <returns>True if the credentials are valid; otherwise false.</returns>
        Task<bool> AreCredentialsValid(INeweggStoreEntity store);

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="orderType">Types of orders to be downloaded.</param>
        /// <returns>A DownloadInfo object.</returns>
        Task<DownloadInfo> GetDownloadInfo(INeweggStoreEntity store, DateTime startingPoint, NeweggOrderType orderType);

        /// <summary>
        /// Gets the download info.
        /// </summary>
        /// <param name="orders">The orders.</param>
        /// <returns>A DownloadInfo object.</returns>
        DownloadInfo GetDownloadInfo(INeweggStoreEntity store, IEnumerable<NeweggOrderEntity> orderEntities);

        /// <summary>
        /// Downloads the specified orders from Newegg
        /// </summary>
        /// <param name="orderEntities">The order entities.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <returns>An IEnumerable of Newegg Order objects.</returns>
        Task<IEnumerable<Order>> DownloadOrders(INeweggStoreEntity store, IEnumerable<NeweggOrderEntity> orderEntities, int pageNumber);

        /// <summary>
        /// Downloads the orders from Newegg.
        /// </summary>
        /// <param name="startingPoint">The starting point.</param>
        /// <param name="endingPoint">The ending point.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="orderType">The type of orders to be downloaded.</param>
        /// <returns>An IEnumerable of Order objects.</returns>
        Task<IEnumerable<Order>> DownloadOrders(INeweggStoreEntity store, Range<DateTime> downloadRange, int pageNumber, NeweggOrderType orderType);

        /// <summary>
        /// Uploads the shipping details.
        /// </summary>
        /// <param name="shipmentEntity">The shipment.</param>
        /// <returns>A ShippingResult containing the results from the request to Newegg.</returns>
        Task<string> UploadShippingDetails(INeweggStoreEntity store, ShipmentEntity shipmentEntity, long orderNumber, IEnumerable<ItemDetails> items);
    }
}