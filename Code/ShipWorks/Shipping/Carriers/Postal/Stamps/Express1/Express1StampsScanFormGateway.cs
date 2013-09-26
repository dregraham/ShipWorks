﻿namespace ShipWorks.Shipping.Carriers.Postal.Stamps.Express1
{
    /// <summary>
    /// An implementation of the IScanFormGateway interface that communicates with the Express1/Stamps API
    /// for creating/obtaining SCAN forms.
    /// </summary>
    public class Express1StampsScanFormGateway : StampsScanFormGateway
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Express1StampsScanFormGateway()
        {
            InvalidCarrierMessage = "An attempt to create an Express1 SCAN form was made for a carrier other than Express1.";
            InvalidShipmentMessage = "Cannot create a Express1 SCAN form for a shipment that was not shipped with Express1.";
        }
    }
}