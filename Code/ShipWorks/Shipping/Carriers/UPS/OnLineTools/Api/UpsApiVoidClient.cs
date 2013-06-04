using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using System.Xml;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api
{
    /// <summary>
    /// UPS API wrapper around being able to void
    /// </summary>
    public static class UpsApiVoidClient
    {
        /// <summary>
        /// Void the given UPS shipment
        /// </summary>
        public static void VoidShipment(ShipmentEntity shipment)
        {
            UpsAccountEntity account = UpsApiCore.GetUpsAccount(shipment);

            // Create the void request writer
            XmlTextWriter xmlWriter = UpsWebClient.CreateRequest(UpsOnLineToolType.ShipVoid, account);

            string trackingNumber = shipment.TrackingNumber;

            // For test server use builtin number
            if (UpsWebClient.UseTestServer)
            {
                trackingNumber = "1Z12345E0390817264";
            }

            // Only valid tag, the tracking number
            xmlWriter.WriteElementString("ShipmentIdentificationNumber", trackingNumber);

            // Process the request - we don't have to do anyting with the response
            UpsWebClient.ProcessRequest(xmlWriter);
        }
    }
}
