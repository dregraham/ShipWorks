using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    /// <summary>
    /// An interface for processing tokenized strings.
    /// </summary>
    public interface ITokenProcessor
    {
        /// <summary>
        /// Processes the specified token text.
        /// </summary>
        /// <param name="tokenText">The token text.</param>
        /// <param name="shipment">The shipment.</param>
        /// <returns>A string value where the tokens have been replaced with actual values.</returns>
        string Process(string tokenText, ShipmentEntity shipment);
    }
}
