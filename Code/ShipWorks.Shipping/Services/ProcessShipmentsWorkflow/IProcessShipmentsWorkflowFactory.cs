namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Factory for creating the correct process shipments workflow
    /// </summary>
    public interface IProcessShipmentsWorkflowFactory
    {
        /// <summary>
        /// Create the correct workflow
        /// </summary>
        IProcessShipmentsWorkflow Create();
    }
}