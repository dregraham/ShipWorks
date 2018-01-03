using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
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
        public string ProcessTokens(string tokenText, IShipmentEntity shipmentEntity)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipmentEntity, nameof(shipmentEntity));

            return TemplateTokenProcessor.ProcessTokens(tokenText, shipmentEntity.ShipmentID);
        }
    }
}
