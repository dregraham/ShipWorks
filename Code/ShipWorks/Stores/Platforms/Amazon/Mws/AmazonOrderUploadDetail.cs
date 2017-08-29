using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.Amazon.Mws
{
    /// <summary>
    /// Details about an amazon order for use in uploading
    /// </summary>
    public class AmazonOrderUploadDetail
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOrderUploadDetail(ShipmentEntity shipment, string amazonOrderID)
        {
            Shipment = shipment;
            AmazonOrderID = amazonOrderID;
        }

        /// <summary>
        /// Shipment
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// Amazon order id
        /// </summary>
        public string AmazonOrderID { get; }
    }
}
