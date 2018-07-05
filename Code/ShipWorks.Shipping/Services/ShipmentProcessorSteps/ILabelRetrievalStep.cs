using System.Threading.Tasks;

namespace ShipWorks.Shipping.Services.ShipmentProcessorSteps
{
    /// <summary>
    /// Step in the process shipment workflow to get the label from the carrier
    /// </summary>
    public interface ILabelRetrievalStep
    {
        /// <summary>
        /// Get a label for a shipment
        /// </summary>
        Task<ILabelRetrievalResult> GetLabel(IShipmentPreparationResult result);
    }
}