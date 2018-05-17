using System.Xml.Linq;
using ShipWorks.Data.Model.EntityInterfaces;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Generates XML for uploading shipment details to Overstock
    /// </summary>
    public interface IOverstockShipmentFactory
    {
        /// <summary>
        /// Create the XML for shipment upload to Overstock
        /// </summary>
        XDocument CreateShipmentDetails(IShipmentEntity shipment);
    }
}