using System.Threading.Tasks;
using System.Xml.XPath;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Platforms.OrderMotion.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.OrderMotion
{
    /// <summary>
    /// Class for communicating with the OrderMotion web service
    /// </summary>
    public interface IOrderMotionWebClient
    {
        /// <summary>
        /// Connects to OrderMotion to test configuration
        /// </summary>
        Task TestConnection(IOrderMotionStoreEntity store);

        /// <summary>
        /// Retrieves an OrderMotion order from the UDI web service
        /// </summary>
        Task<IXPathNavigable> GetOrder(IOrderMotionStoreEntity store, long orderNumber);

        /// <summary>
        /// Makes an ItemInformationRequest to OrderMotion
        /// </summary>
        Task<IXPathNavigable> GetItemInformation(IOrderMotionStoreEntity store, string itemCode);

        /// <summary>
        /// Upload tracking information to OrderMotion
        /// </summary>
        Task UploadShipmentDetails(IOrderMotionStoreEntity store, ShipmentEntity shipment, OrderDetail order);
    }
}