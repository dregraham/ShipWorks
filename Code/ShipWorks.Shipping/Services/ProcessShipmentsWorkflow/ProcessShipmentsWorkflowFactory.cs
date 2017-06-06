using System;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Shipping.Services.ProcessShipmentsWorkflow
{
    /// <summary>
    /// Factory for creating the correct process shipments workflow
    /// </summary>
    /// <remarks>Right now, this doesn't do much but in a few months, this should be where we add the code to
    /// return either a serial or a parallel processing workflow</remarks>
    [Component]
    public class ProcessShipmentsWorkflowFactory : IProcessShipmentsWorkflowFactory
    {
        private readonly Func<SerialProcessShipmentsWorkflow> createSerialWorkflow;
        private readonly Func<ParallelProcessShipmentsWorkflow> createParallelWorkflow;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentsWorkflowFactory(
            Func<SerialProcessShipmentsWorkflow> createSerialWorkflow,
            Func<ParallelProcessShipmentsWorkflow> createParallelWorkflow)
        {
            this.createParallelWorkflow = createParallelWorkflow;
            this.createSerialWorkflow = createSerialWorkflow;
        }

        /// <summary>
        /// Create the correct workflow
        /// </summary>
        public IProcessShipmentsWorkflow Create(int shipmentCount) =>
            shipmentCount == 1 ? (IProcessShipmentsWorkflow) createSerialWorkflow() : createParallelWorkflow();
    }
}
