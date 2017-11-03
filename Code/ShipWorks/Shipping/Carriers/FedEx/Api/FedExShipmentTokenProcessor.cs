using System;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Tokens;

namespace ShipWorks.Shipping.Carriers.FedEx.Api
{
    /// <summary>
    /// An implementation of the IFedExShipmentTokenProcessor interface.
    /// </summary>
    [Component]
    public class FedExShipmentTokenProcessor : IFedExShipmentTokenProcessor
    {
        /// <summary>
        /// Processes the token text provided into a formatted string based on the shipment data.
        /// </summary>
        /// <param name="tokenText">The token text.</param>
        /// <param name="shipmentEntity">The shipment entity.</param>
        /// <returns>A formatted string having the token values replaced with values from the shipment.</returns>
        public string ProcessTokens(string tokenText, ShipmentEntity shipmentEntity)
        {
            if (shipmentEntity == null)
            {
                throw new ArgumentNullException("shipmentEntity");
            }

            return TemplateTokenProcessor.ProcessTokens(tokenText, shipmentEntity.ShipmentID);
        }
    }
}
