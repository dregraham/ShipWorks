using System;
using System.Threading.Tasks;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericModule
{
    /// <summary>
    /// Class for connecting to and working with our Generic php module
    /// </summary>
    public interface IGenericStoreWebClient
    {
        /// <summary>
        /// Get the module and capabilities information from the module
        /// </summary>
        GenericModuleResponse GetModule();

        /// <summary>
        /// Downloads the next batch of orders from the web module, started on the lastModified time.
        /// </summary>
        GenericModuleResponse GetNextOrderPage(DateTime? lastModified);

        /// <summary>
        /// Downloads the next batch of orders from Generic, starting at the lastOrderNumber
        /// </summary>
        GenericModuleResponse GetNextOrderPage(long lastOrderNumber);

        /// <summary>
        /// Get the number of orders that are newer than lastModified
        /// </summary>
        int GetOrderCount(DateTime? lastModified);

        /// <summary>
        /// Gets the number of orders ready to download
        /// </summary>
        int GetOrderCount(long lastOrderNumber);

        /// <summary>
        /// Retrieves the status code definitions from the online store
        /// </summary>
        GenericModuleResponse GetStatusCodes();

        /// <summary>
        /// Get the store details from the online store
        /// </summary>
        GenericModuleResponse GetStore();

        /// <summary>
        /// Loads a GenericModuleResponse from the specified file
        /// </summary>
        GenericModuleResponse ResponseFromFile(string file, string action);

        /// <summary>
        /// Update the online status of the specified order
        /// </summary>
        Task UpdateOrderStatus(OrderEntity order, object code, string comment);

        /// <summary>
        /// Update the online status of the specified order
        /// </summary>
        Task UploadShipmentDetails(OrderEntity order, ShipmentEntity shipment);
    }
}