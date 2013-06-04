using System;
using ShipWorks.Templates.Tokens;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// An implementation of the ITokenProcessor interface that uses the TemplateTokenProcessor.
    /// </summary>
    public class iParcelTokenProcessor : ITokenProcessor
    {
        /// <summary>
        /// Processes the specified token text.
        /// </summary>
        /// <param name="tokenText">The token text.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A string value where the tokens have been replaced with actual values.</returns>
        public string Process(string tokenText, ShipmentEntity shipment)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            return TemplateTokenProcessor.ProcessTokens(tokenText, shipment.ShipmentID);
        }
    }
}
