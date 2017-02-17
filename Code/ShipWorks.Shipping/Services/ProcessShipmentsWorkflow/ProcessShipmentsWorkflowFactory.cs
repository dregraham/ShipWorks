using System;
using ShipWorks.ApplicationCore.ComponentRegistration;

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
        readonly Func<SerialProcessShipmentsWorkflow> createSerialWorkflow;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProcessShipmentsWorkflowFactory(Func<SerialProcessShipmentsWorkflow> createSerialWorkflow)
        {
            this.createSerialWorkflow = createSerialWorkflow;
        }

        /// <summary>
        /// Create the correct workflow
        /// </summary>
        public IProcessShipmentsWorkflow Create() => createSerialWorkflow();
    }
}
