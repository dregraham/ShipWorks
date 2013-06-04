﻿using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.v2013
{
    /// <summary>
    /// An interface for processing tokens into a formatted string based on shipment data.
    /// </summary>
    public interface IFedExShipmentTokenProcessor
    {
        /// <summary>
        /// Processes the tokens.
        /// </summary>
        /// <param name="tokenText">The token text.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A formatted string having the token values replaced with values from the shipment.</returns>
        string ProcessTokens(string tokenText, ShipmentEntity shipmentEntity);
    }
}
