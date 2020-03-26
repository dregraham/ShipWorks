using System;
using System.Collections.Generic;
using System.Linq;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Win32;
using ShipWorks.Data.Model.EntityClasses;

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
        public IProcessShipmentsWorkflow Create(IEnumerable<ShipmentEntity> shipments)
        {
            if ((shipments.Count() > 1 || shipments.First().IncludeReturn) && !ShouldForceSerial())
            {
                return workflows[ProcessShipmentsWorkflow.Parallel];
            }
            else
            {
                return workflows[ProcessShipmentsWorkflow.Serial];
            }
        }

        /// <summary>
        /// Should we force serial processing
        /// </summary>
        /// <returns></returns>
        private bool ShouldForceSerial() =>
            new RegistryHelper(ParallelProcessShipmentsWorkflow.LabelProcessingConcurrencyBasePath)
                .GetValue("forceSerial", false);
    }
}
