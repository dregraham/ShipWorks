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
        public AmazonOrderUploadDetail(ShipmentEntity shipment, AmazonOrderEntity order)
        {
            Shipment = shipment;
            AmazonOrderID = order.AmazonOrderID;
            IsManual = order.IsManual;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonOrderUploadDetail(ShipmentEntity shipment, string amazonOrderID, bool isManual)
        {
            Shipment = shipment;
            AmazonOrderID = amazonOrderID;
            IsManual = isManual;
        }

        /// <summary>
        /// Shipment
        /// </summary>
        public ShipmentEntity Shipment { get; }

        /// <summary>
        /// Amazon order id
        /// </summary>
        public string AmazonOrderID { get; }

        /// <summary>
        /// Is the order manual
        /// </summary>
        public bool IsManual { get; }
    }
}
