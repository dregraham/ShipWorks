using System.Xml.XPath;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Volusion
{
    /// <summary>
    /// Web client for the Volusion API.
    /// </summary>
    /// <remarks>
    /// Not to be confused with the VolusionWebSession class which is used
    /// for screen-scraping connection details for the api
    /// </remarks>
    public interface IVolusionWebClient
    {
        /// <summary>
        /// Tests credentials against the store to see if they are valid
        /// </summary>
        bool ValidateCredentials(IVolusionStoreEntity store);

        /// <summary>
        /// Returns customer information for customer id
        /// </summary>
        IXPathNavigable GetCustomer(IVolusionStoreEntity store, long customerId);

        /// <summary>
        /// Downloads orders that are Ready To Ship
        /// </summary>
        IXPathNavigable GetOrders(IVolusionStoreEntity store, string status);

        /// <summary>
        /// Uploads shipment details to Volusion
        /// </summary>
        void UploadShipmentDetails(IVolusionStoreEntity store, long orderNumber, ShipmentEntity shipment, bool sendEmail);
    }
}