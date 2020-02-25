using System;
using System.Collections.Generic;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Services.ProcessShipmentsWorkflow;

namespace ShipWorks.Api.Orders.Shipments
{
    /// <summary>
    /// Factory for creating the compatible shipments workflow for the API
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ApiProcessShipmentsWorkflowFactory : IProcessShipmentsWorkflowFactory
    {
        private readonly IIndex<ProcessShipmentsWorkflow, IProcessShipmentsWorkflow> workflows;

        /// <summary>
        /// Constructor
        /// </summary>
        public ApiProcessShipmentsWorkflowFactory(IIndex<ProcessShipmentsWorkflow, IProcessShipmentsWorkflow> workflows)
        {
            this.workflows = workflows;
        }

        /// <summary>
        /// Create the correct workflow
        /// </summary>
        public IProcessShipmentsWorkflow Create(IEnumerable<ShipmentEntity> shipments)
        {
            return workflows[ProcessShipmentsWorkflow.Serial];
        }
    }
}
