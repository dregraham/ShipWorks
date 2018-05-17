using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Stores.Platforms.Overstock.OnlineUpdating;

namespace ShipWorks.Stores.Platforms.Overstock
{
    /// <summary>
    /// Generates XML for uploading shipment details to Overstock
    /// </summary>
    [Component]
    public class OverstockShipmentFactory : IOverstockShipmentFactory
    {
        /// <summary>
        /// Create the XML for shipment upload to Overstock
        /// </summary>
        public XDocument CreateShipmentDetails(IEnumerable<OverstockSupplierShipment> shipments)
        {
            XNamespace ns = "api.supplieroasis.com";

            XDocument shipmentDetails = new XDocument(
                new XDeclaration("1.0", "UTF-8", "yes"),
                new XElement(ns + "supplierShipmentMessage",
                    shipments.Select(oi => new XElement("supplierShipment",
                            new XElement("salesChannelName", oi.SalesChannelName),
                            new XElement("salesChannelOrderNumber", oi.SalesChannelOrderNumber),
                            new XElement("salesChannelLineNumber", oi.SalesChannelLineNumber),
                            new XElement("warehouse", new XElement("code", oi.WarehouseCode)),
                            new XElement("supplierShipConfirmation",
                                new XElement("quantity", oi.Quantity),
                                new XElement("carrier", new XElement("code", oi.CarrierCode)),
                                new XElement("trackingNumber", oi.TrackingNumber),
                                new XElement("shipDate", oi.ShipDate.ToString("o")),
                                new XElement("serviceLevel", new XElement("code", oi.ServiceLevelCode)))
                        )
                    )
                )
            );

            return shipmentDetails;
        }
    }
}
